using InventoryTracker.APIClient;
using InventoryTracker.Contracts.Responses;

namespace InventoryTracker.WebOperator.Providers
{
    public class HttpContextTokenStore : ITokenStore
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextTokenStore(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void StoreTokens(AuthResponseDTO tokens)
        {
            var response = _httpContextAccessor.HttpContext?.Response;

            if (response is null)
                return;

            response.Cookies.Append("accessToken", tokens.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = tokens.AccessTokenExpiresAtUtc
            });

            response.Cookies.Append("refreshToken", tokens.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = tokens.RefreshTokenExpiresAtUtc
            });
        }

        public void ClearTokens()
        {
            var response = _httpContextAccessor.HttpContext?.Response;

            if (response is null)
                return;

            response.Cookies.Delete("accessToken");
            response.Cookies.Delete("refreshToken");
        }
    }
}
