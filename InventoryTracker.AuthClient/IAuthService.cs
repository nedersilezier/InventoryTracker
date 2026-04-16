using InventoryTracker.Contracts.Responses;
using InventoryTracker.Contracts.Requests.Auth;


namespace InventoryTracker.AuthClient
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> LoginAsync(LoginViewModel request, CancellationToken cancellationToken);
        Task LogoutAsync(LogoutRequest request, CancellationToken cancellationToken);
        Task<AuthResponseDTO?> RefreshTokenAsync(TokenRefreshRequest request, CancellationToken cancellationToken);
    }
}
