using InventoryTracker.Contracts.Requests.Items;
using InventoryTracker.Contracts.Responses.Items;
using InventoryTracker.WebAdmin.Helpers;
using InventoryTracker.WebAdmin.ViewModels.Items;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface IItemsService
    {
        Task<ItemsIndexViewModel> GetAllAsync(GetItemsRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<CreateItemResponse>> CreateItemAsync(CreateItemRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<CreateEditItemViewModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ServiceResult<CreateItemResponse>> UpdateItemAsync(Guid id, UpdateItemRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<ItemDetailsViewModel>> GetDetailsByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ServiceResult<CreateItemResponse>> DeactivateItemAsync(Guid id, CancellationToken cancellationToken);
        Task<ServiceResult<CreateItemResponse>> ActivateItemAsync(Guid id, CancellationToken cancellationToken);
    }
}
