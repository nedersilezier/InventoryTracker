using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Users;
using InventoryTracker.Contracts.Responses.Users;
using InventoryTracker.WebAdmin.ViewModels.Users;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface IUsersService
    {
        Task<ServiceResult<UsersIndexViewModel>> GetAllAsync(GetUsersRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<UserCreatedResponse>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken);
         //Task<UserDTO> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
         //Task<UserDTO> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken);
         //Task<UserDTO> UpdateUserAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken);
    }
}
