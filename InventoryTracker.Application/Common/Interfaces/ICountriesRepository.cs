using InventoryTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface ICountriesRepository
    {
        Task<bool> CountryExistsAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> CountryCodeExistsAsync(string code, CancellationToken cancellationToken);
        Task<bool> CountryCodeExistsForUpdateAsync(string code, Guid id, CancellationToken cancellationToken);
        Task<Country?> GetCountryByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddCountry(Country country, CancellationToken cancellationToken);
        Task DeleteCountry(Country country, CancellationToken cancellationToken);
    }
}
