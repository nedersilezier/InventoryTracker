using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Infrastructure.Repositories
{
    public class CountriesRepository: ICountriesRepository
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
        public async Task<Country?> GetCountryByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Countries.FirstOrDefaultAsync(c => c.CountryId == id, cancellationToken);
        }
    }
}
