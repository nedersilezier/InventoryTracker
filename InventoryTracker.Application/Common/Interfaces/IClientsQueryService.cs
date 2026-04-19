using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Clients.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface IClientsQueryService
    {
        Task<PagedResult<ClientDTO>> GetAllClientsAsync(GetClientsParameters parameters, CancellationToken cancellationToken);
        Task<ClientDTO?> GetClientByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
