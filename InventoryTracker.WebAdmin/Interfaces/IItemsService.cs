using InventoryTracker.Contracts.Requests.Items;
using InventoryTracker.WebAdmin.ViewModels.Items;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface IItemsService
    {
        Task<ItemsIndexViewModel> GetAllAsync(GetItemsRequest request, CancellationToken cancellationToken);
    }
}
