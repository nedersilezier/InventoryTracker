using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Infrastructure.Repositories
{
    public class AddressesRepository : IAddressesRepository
    {
        private readonly AppDbContext _context;
        public AddressesRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task AddAddress(Address address)
        {
            _context.Addresses.Add(address);
            return Task.CompletedTask;
        }
        public async Task<bool> AddressExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Addresses.AnyAsync(a => a.AddressId == id, cancellationToken);
        }
        public async Task<Address?> GetAddressByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Addresses.FirstOrDefaultAsync(a => a.AddressId == id, cancellationToken);
        }
        public async Task<bool> HasAnyForCountryAsync(Guid countryId, CancellationToken cancellationToken)
        {
            return await _context.Addresses.AnyAsync(a => a.CountryId == countryId, cancellationToken);
        }
    }
}
