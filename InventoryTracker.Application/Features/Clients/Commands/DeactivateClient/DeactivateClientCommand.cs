using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Clients.Commands.DeactivateClient
{
    public class DeactivateClientCommand: IRequest<ClientDTO?>
    {
        public Guid ClientId { get; private set; }
        public DeactivateClientCommand(Guid clientId)
        {
            ClientId = clientId;
        }
    }
}
