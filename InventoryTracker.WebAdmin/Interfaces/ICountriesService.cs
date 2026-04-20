using InventoryTracker.Contracts.Requests.Countries;
using InventoryTracker.Contracts.Responses.Countries;
using InventoryTracker.WebAdmin.ViewModels.Countries;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface ICountriesService
    {
        //Task<IEnumerable<CountryDTO>> GetAllAsync(CancellationToken cancellationToken);
        //Task<CountriesIndexViewModel> GetAllAsync(CancellationToken cancellationToken);
        Task<CountriesIndexViewModel> GetAllAsync(GetCountriesRequest request, CancellationToken cancellationToken);
        Task<CountryResponseDTO> GetCountryByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<CountryResponseDTO> CreateCountryAsync(CreateCountryRequest request, CancellationToken cancellationToken);
        Task<CountryResponseDTO> UpdateCountryAsync(Guid id, UpdateCountryRequest request, CancellationToken cancellationToken);
    }
}
