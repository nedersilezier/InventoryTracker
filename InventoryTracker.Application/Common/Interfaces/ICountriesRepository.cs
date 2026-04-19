using InventoryTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface ICountriesRepository
    {
        Task<bool> CountryExistsAsync(Guid id, CancellationToken cancellationToken);
        Task<Country?> GetCountryByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
