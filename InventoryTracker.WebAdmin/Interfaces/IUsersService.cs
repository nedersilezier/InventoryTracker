using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Users;
using InventoryTracker.Contracts.Responses.Users;
using InventoryTracker.WebAdmin.ViewModels.Users;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface IUsersService
    {
        Task<ServiceResult<UsersIndexViewModel>> GetAllAsync(GetUsersRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<EditUserViewModel>> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<ServiceResult<UserCreatedResponse>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<UserCreatedResponse>> UpdateUserAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<UserCreatedResponse>> ActivateUserAsync(string id, CancellationToken cancellationToken);
        Task<ServiceResult<UserCreatedResponse>> DeactivateUserAsync(string id, CancellationToken cancellationToken);
    }
}
