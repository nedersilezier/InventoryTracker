using System.Net.Http.Headers;

namespace InventoryTracker.APIClient
{
    public class AccessTokenHandler : DelegatingHandler
    {
        private readonly IAccessTokenProvider _tokenProvider;

        public AccessTokenHandler(IAccessTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _tokenProvider.GetAccessToken();

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
