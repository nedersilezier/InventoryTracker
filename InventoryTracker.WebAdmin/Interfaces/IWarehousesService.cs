using InventoryTracker.Contracts.Requests.Warehouses;
using InventoryTracker.WebAdmin.ViewModels.Warehouses;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface IWarehousesService
    {
        Task<WarehousesIndexViewModel> GetAllAsync(GetWarehousesRequest request, CancellationToken cancellationToken);
    }
}
