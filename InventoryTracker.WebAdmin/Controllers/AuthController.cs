using InventoryTracker.Contracts.Requests.Auth;
using InventoryTracker.WebAdmin.Exceptions;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.Login;
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
        public IActionResult Login()
        {
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
                if (result == null) throw new Exception("Login failed without error message.");

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

                return RedirectToAction("Index", "Home");
            }
            catch (ApiValidationException ex)
            {
                foreach(var problem in  ex.Errors)
                {
                    var key = problem.Key;
                    foreach(var message in problem.Value)
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
    }
}
