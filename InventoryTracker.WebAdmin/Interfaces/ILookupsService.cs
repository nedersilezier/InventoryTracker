using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Responses.Countries;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface ILookupsService
    {
        Task<ServiceResult<List<CountryResponseSelectDTO>>> GetCountriesAsync(CancellationToken cancellationToken);
    }
}
