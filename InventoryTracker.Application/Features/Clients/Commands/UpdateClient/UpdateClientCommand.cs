using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Clients.DTOs;
using InventoryTracker.Application.Features.Items.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Clients.Commands.UpdateClient
{
    public class UpdateClientCommand: IRequest<ClientDTO?>
    {
        public Guid ClientId { get; set; }
        public string Name { get; set; } = default!;
        public string ClientCode { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public UpdateClientAddressDTO Address { get; set; } = default!;
        public class UpdateClientAddressDTO
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
