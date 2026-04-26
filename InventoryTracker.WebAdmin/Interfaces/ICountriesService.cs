using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Countries;
using InventoryTracker.Contracts.Responses.Countries;
using InventoryTracker.WebAdmin.ViewModels.Countries;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface ICountriesService
    {
        Task<ServiceResult<CountriesIndexViewModel>> GetAllAsync(GetCountriesRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<CreateEditCountryViewModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ServiceResult<CreateCountryResponse>> CreateCountryAsync(CreateCountryRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<CreateCountryResponse>> UpdateCountryAsync(Guid id, UpdateCountryRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<object>> DeleteCountryAsync(Guid id, CancellationToken cancellationToken);
    }
}
