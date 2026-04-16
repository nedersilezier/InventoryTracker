using InventoryTracker.Contracts.Requests.Auth;
using InventoryTracker.Contracts.Responses;
using InventoryTracker.AuthClient;
using InventoryTracker.AuthClient.Exceptions;

using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class AuthController : Controller
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
                try
                {
                    var result = await _authService.RefreshTokenAsync(new TokenRefreshRequest { RefreshToken = refreshToken }, cancellationToken);
                    if (result != null)
                    {
                        AppendAuthCookies(result);
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                catch
                {
                    Response.Cookies.Delete("accessToken");
                    Response.Cookies.Delete("refreshToken");
                }
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
            try
            {
                var result = await _authService.LoginAsync(request, cancellationToken);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Login failed.");
                    return View(request);
                }
                AppendAuthCookies(result);
                return RedirectToAction("Index", "Dashboard");
            }
            catch (ApiValidationException ex)
            {
                foreach (var problem in ex.Errors)
                {
                    var key = problem.Key;
                    foreach (var message in problem.Value)
                    {
                        if (string.Equals(key, "General", StringComparison.OrdinalIgnoreCase))
                        {
                            ModelState.AddModelError(string.Empty, message);
                        }
                        else
                        {
                            ModelState.AddModelError(key, message);
                        }
                    }
                }
                return View(request);
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(request);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                try
                {
                    await _authService.LogoutAsync(new LogoutRequest { RefreshToken = refreshToken }, cancellationToken);
                }
                catch
                {
                    // Ignore exceptions during logout
                }
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
