using InventoryTracker.APIClient;
using InventoryTracker.AuthClient;
using InventoryTracker.Contracts.Requests.Auth;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebOperator.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ITokenStore _tokenStore;
        public AuthController(IAuthService authService, ITokenStore tokenStore)
        {
            _authService = authService;
            _tokenStore = tokenStore;
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
                    _tokenStore.StoreTokens(result.Data!);
                    return RedirectToAction("Index", "Transactions");
                }
                _tokenStore.ClearTokens();
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
            _tokenStore.StoreTokens(result.Data!);
            return RedirectToAction("Index", "Transactions");
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

            _tokenStore.ClearTokens();
            return RedirectToAction("Login", "Auth");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
