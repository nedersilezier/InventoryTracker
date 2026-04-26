using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Responses.Countries;
using InventoryTracker.Contracts.Responses.Warehouses;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface ILookupsService
    {
        Task<ServiceResult<List<CountryResponseSelectDTO>>> GetCountriesAsync(CancellationToken cancellationToken);
        Task<ServiceResult<List<WarehouseResponseSelectDTO>>> GetWarehousesAsync(CancellationToken cancellationToken);
    }
}
