using InventoryTracker.Contracts.Responses;
using InventoryTracker.Contracts.Requests.Auth;
using InventoryTracker.WebAdmin.ViewModels.Login;


namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> LoginAsync(LoginViewModel request, CancellationToken cancellationToken);
        Task LogoutAsync(LogoutRequest request);
    }
}
