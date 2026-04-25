using InventoryTracker.Contracts.Responses;
using InventoryTracker.Contracts.Requests.Auth;
using InventoryTracker.Contracts.Helpers;


namespace InventoryTracker.AuthClient
{
    public interface IAuthService
    {
        Task<ServiceResult<AuthResponseDTO>> LoginAsync(LoginViewModel request, CancellationToken cancellationToken);
        Task<ServiceResult> LogoutAsync(LogoutRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<AuthResponseDTO>> RefreshTokenAsync(TokenRefreshRequest request, CancellationToken cancellationToken);
    }
}
