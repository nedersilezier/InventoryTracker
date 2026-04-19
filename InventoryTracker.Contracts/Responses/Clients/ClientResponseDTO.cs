using InventoryTracker.Contracts.Responses.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Clients
{
    public class ClientResponseDTO
    {
        public Guid ClientId { get; set; }
        public string Name { get; set; } = default!;
        public string ClientCode { get; set; } = default!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }

        public AddressResponseDTO Address { get; set; } = default!;
    }
}
