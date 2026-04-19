using InventoryTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface IAddressesRepository
    {
        Task AddAddress(Address address);
        Task<bool> AddressExistsAsync(Guid id, CancellationToken cancellationToken);
        Task<Address?> GetAddressByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
