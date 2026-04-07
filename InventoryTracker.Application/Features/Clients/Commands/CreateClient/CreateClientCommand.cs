using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Clients.Commands.CreateClient
{
    public class CreateClientCommand: IRequest<ClientDTO>
    {
        public string Name { get; set; } = default!;
        public string ClientCode { get; set; } = default!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public CreateClientAddressDTO Address { get; set; } = default!;
        public class CreateClientAddressDTO
        {
            public string Street { get; set; } = default!;
            public string HouseNumber { get; set; } = default!;
            public string? ApartmentNumber { get; set; }
            public string PostalCode { get; set; } = default!;
            public string City { get; set; } = default!;
            public Guid CountryId { get; set; }
        }
    }
}
