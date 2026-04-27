using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Clients.Queries.GetById
{
    public class GetClientByIdQuery : IRequest<ClientDTO?>
    {
        public Guid ClientId { get; set; }
        public GetClientByIdQuery(Guid clientId)
        {
            ClientId = clientId;
        }
    }
}
