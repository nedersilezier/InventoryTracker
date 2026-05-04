using InventoryTracker.APIClient;

namespace InventoryTracker.WebOperator.Providers
{
    public class HttpContextRefreshTokenProvider : IRefreshTokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextRefreshTokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetRefreshToken()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
        }
    }
}
