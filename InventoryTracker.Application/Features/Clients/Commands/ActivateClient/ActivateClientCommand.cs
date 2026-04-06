using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Clients.Commands.ActivateClient
{
    public class ActivateClientCommand: IRequest<ClientDTO?>
    {
        public Guid ClientId { get; private set; }
        public ActivateClientCommand(Guid clientId)
        {
            ClientId = clientId;
        }
    }
}
