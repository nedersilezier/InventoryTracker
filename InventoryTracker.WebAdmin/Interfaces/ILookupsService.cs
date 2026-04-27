using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Responses.Clients;
using InventoryTracker.Contracts.Responses.Countries;
using InventoryTracker.Contracts.Responses.Items;
using InventoryTracker.Contracts.Responses.Users;
using InventoryTracker.Contracts.Responses.Warehouses;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface ILookupsService
    {
        Task<ServiceResult<List<CountryResponseSelectDTO>>> GetCountriesAsync(CancellationToken cancellationToken);
        Task<ServiceResult<List<WarehouseResponseSelectDTO>>> GetWarehousesAsync(CancellationToken cancellationToken);
        Task<ServiceResult<List<ClientResponseSelectDTO>>> GetClientsAsync(CancellationToken cancellationToken);
        Task<ServiceResult<List<ItemResponseSelectDTO>>> GetItemsAsync(CancellationToken cancellationToken);
        Task<ServiceResult<List<RoleResponseDTO>>> GetRolesAsync(CancellationToken cancellationToken);
    }
}
