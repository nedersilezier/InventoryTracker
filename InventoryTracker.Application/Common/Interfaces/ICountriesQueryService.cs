using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Countries.DTOs;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface ICountriesQueryService
    {
        Task<PagedResult<CountryDTO>> GetAllCountriesAsync(GetCountriesParameters parameters, CancellationToken cancellationToken);
        Task<IReadOnlyList<InternalCountrySelectDTO>> GetAllCountriesLookupAsync(CancellationToken cancellationToken);
        Task<CountryDTO?> GetCountryByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
