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
    }
}
