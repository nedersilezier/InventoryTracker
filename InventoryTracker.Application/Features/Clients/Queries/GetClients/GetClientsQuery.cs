using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Clients.Queries.GetClients
{
    public class GetClientsQuery: IRequest<PagedResult<ClientDTO>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; } = 1;
        public string? SearchTerm { get; set; }
    }
}
