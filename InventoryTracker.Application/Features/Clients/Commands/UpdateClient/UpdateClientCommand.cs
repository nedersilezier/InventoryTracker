using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Clients.DTOs;
using InventoryTracker.Application.Features.Items.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Clients.Commands.UpdateClient
{
    public class UpdateClientCommand: IRequest<ClientDTO?>
    {
        public Guid ClientId { get; set; }
        public string Name { get; set; } = default!;
        public string ClientCode { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public AddressDTO Address { get; set; } = default!;
    }
}
