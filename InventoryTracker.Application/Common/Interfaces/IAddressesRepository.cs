using InventoryTracker.Domain.Entities;


namespace InventoryTracker.Application.Common.Interfaces
{
    public interface IAddressesRepository
    {
        Task AddAddress(Address address);
        Task<bool> AddressExistsAsync(Guid id, CancellationToken cancellationToken);
        Task<Address?> GetAddressByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> HasAnyForCountryAsync(Guid countryId, CancellationToken cancellationToken);
    }
}
