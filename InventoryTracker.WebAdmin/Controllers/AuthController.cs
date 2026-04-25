using InventoryTracker.Contracts.Requests.Auth;
using InventoryTracker.Contracts.Responses;
using InventoryTracker.AuthClient;

using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet]
        public async Task<IActionResult> Login(CancellationToken cancellationToken)
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var result = await _authService.RefreshTokenAsync(new TokenRefreshRequest { RefreshToken = refreshToken }, cancellationToken);
                if (result.Success)
                {
                    AppendAuthCookies(result.Data!);
                    return RedirectToAction("Index", "Dashboard");
                }
                Response.Cookies.Delete("accessToken");
                Response.Cookies.Delete("refreshToken");
            }
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _authService.LoginAsync(request, cancellationToken);
            if (!result.Success)
            {
                AddServiceErrorsToModelState(result);
                return View(request);
            }
            AppendAuthCookies(result.Data!);
            return RedirectToAction("Index", "Dashboard");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _authService.LogoutAsync(
                    new LogoutRequest { RefreshToken = refreshToken },
                    cancellationToken);
            }

            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");

            return RedirectToAction("Login", "Auth");
        }

        private void AppendAuthCookies(AuthResponseDTO result)
        {
            Response.Cookies.Append("accessToken", result.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = result.AccessTokenExpiresAtUtc
            });

            Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = result.RefreshTokenExpiresAtUtc
            });
        }
    }
}
