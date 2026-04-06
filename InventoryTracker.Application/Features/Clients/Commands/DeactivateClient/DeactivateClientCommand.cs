using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

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
