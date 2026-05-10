using System.Net;
using System.Net.Http.Headers;

namespace InventoryTracker.APIClient
{
    public class AccessTokenHandler : DelegatingHandler
    {
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;
        private readonly ITokenRefreshClient _tokenRefreshClient;
        private readonly ITokenStore _authTokenStore;

        public AccessTokenHandler(IAccessTokenProvider accessTokenProvider, IRefreshTokenProvider refreshTokenProvider, ITokenRefreshClient tokenRefreshClient, ITokenStore authTokenStore)
        {
            _accessTokenProvider = accessTokenProvider;
            _refreshTokenProvider = refreshTokenProvider;
            _tokenRefreshClient = tokenRefreshClient;
            _authTokenStore = authTokenStore;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var retryRequest = await CloneHttpRequestMessageAsync(request, cancellationToken);
            AddBearerToken(request, _accessTokenProvider.GetAccessToken());

            var response = await base.SendAsync(request, cancellationToken);
            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return response;
            response.Dispose();

            var refreshToken = _refreshTokenProvider.GetRefreshToken();
            if (string.IsNullOrWhiteSpace(refreshToken))
                return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    RequestMessage = request
                };

            var refreshResult = await _tokenRefreshClient.RefreshAsync(refreshToken, cancellationToken);
            if (!refreshResult.Success || refreshResult.Data is null)
            {
                _authTokenStore.ClearTokens();
                return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    RequestMessage = request
                };
            }

            _authTokenStore.StoreTokens(refreshResult.Data);
            AddBearerToken(retryRequest, refreshResult.Data.AccessToken);

            return await base.SendAsync(retryRequest, cancellationToken);
        }

        private static void AddBearerToken(HttpRequestMessage request, string? accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return;

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        private static async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Version = request.Version,
                VersionPolicy = request.VersionPolicy
            };

            foreach (var header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (request.Content is not null)
            {
                var bytes = await request.Content.ReadAsByteArrayAsync(cancellationToken);
                clone.Content = new ByteArrayContent(bytes);

                foreach (var header in request.Content.Headers)
                {
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            return clone;
        }
    }
}
