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
    }
}
