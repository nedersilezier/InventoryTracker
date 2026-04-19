using InventoryTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface IClientsRepository
    {
        Task<bool> ClientCodeExistsAsync(string clientCode, CancellationToken cancellationToken);
        Task AddClient(Client client);
        Task<Client?> GetClientByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
