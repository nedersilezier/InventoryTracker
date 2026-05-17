using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Infrastructure.Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly AppDbContext _context;
        public CountriesRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CountryExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Countries.AnyAsync(c => c.CountryId == id, cancellationToken);
        }
        public async Task<bool> CountryCodeExistsAsync(string code, CancellationToken cancellationToken)
        {
            return await _context.Countries.AnyAsync(c => c.Code == code, cancellationToken);
        }
        public async Task<bool> CountryCodeExistsForUpdateAsync(string code, Guid id, CancellationToken cancellationToken)
        {
            return await _context.Countries.AnyAsync(c => c.Code == code && c.CountryId != id, cancellationToken);
        }
        public async Task<Country?> GetCountryByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Countries.FirstOrDefaultAsync(c => c.CountryId == id, cancellationToken);
        }
        public Task AddCountry(Country country, CancellationToken cancellationToken)
        {
            _context.Countries.Add(country);
            return Task.CompletedTask;
        }
        public Task DeleteCountry(Country country, CancellationToken cancellationToken)
        {
            _context.Countries.Remove(country);
            return Task.CompletedTask;
        }
    }
}
