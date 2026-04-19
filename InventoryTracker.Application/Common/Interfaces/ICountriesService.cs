using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Countries.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface ICountriesService
    {
        Task<PagedResult<CountryDTO>> GetAllCountriesAsync(GetCountriesParameters parameters, CancellationToken cancellationToken);
        Task<CountryDTO> CreateCountryAsync(CreateCountryParameters parameters, CancellationToken cancellationToken);
        Task<CountryDTO?> UpdateCountryAsync(UpdateCountryParameters parameters, CancellationToken cancellationToken);
        Task DeleteCountryAsync(Guid id, CancellationToken cancellationToken);
        Task<CountryDTO?> GetCountryById(Guid id, CancellationToken cancellationToken);
    }
}
