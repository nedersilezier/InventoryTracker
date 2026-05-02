using InventoryTracker.APIClient;
using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Auth;
using InventoryTracker.Contracts.Responses;

namespace InventoryTracker.AuthClient
{
    public class TokenRefreshClient : ITokenRefreshClient
    {
        private readonly IAuthService _authService;

        public TokenRefreshClient(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<ServiceResult<AuthResponseDTO>> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
        {
            return _authService.RefreshTokenAsync(new TokenRefreshRequest { RefreshToken = refreshToken }, cancellationToken);
        }
    }
}
