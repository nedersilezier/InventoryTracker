using InventoryTracker.Contracts.Requests.Countries;

using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface ICountryService
    {
        Task<IActionResult> GetAllCountries(CancellationToken cancellationToken);
        Task<IActionResult> GetCountryById(Guid id, CancellationToken cancellationToken);
        Task<IActionResult> CreateCountry(CreateCountryRequest request, CancellationToken cancellationToken);
        Task<IActionResult> UpdateCountry(Guid id, UpdateCountryRequest request, CancellationToken cancellationToken);
    }
}
