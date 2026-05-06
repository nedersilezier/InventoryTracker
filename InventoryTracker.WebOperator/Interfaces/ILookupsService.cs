using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Responses.Clients;
using InventoryTracker.Contracts.Responses.Items;
using InventoryTracker.Contracts.Responses.Warehouses;

namespace InventoryTracker.WebOperator.Interfaces
{
    public interface ILookupsService
    {
        Task<ServiceResult<List<WarehouseResponseSelectDTO>>> GetWarehousesAsync(CancellationToken cancellationToken);
        Task<ServiceResult<List<ClientResponseSelectDTO>>> GetClientsAsync(CancellationToken cancellationToken);
        Task<ServiceResult<List<ItemResponseSelectDTO>>> GetItemsAsync(CancellationToken cancellationToken);
    }
}
