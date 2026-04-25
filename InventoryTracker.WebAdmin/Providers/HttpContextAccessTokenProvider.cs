using InventoryTracker.APIClient;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Providers
{
    public class HttpContextAccessTokenProvider : IAccessTokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpContextAccessTokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetAccessToken()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];
        }
    }
}
