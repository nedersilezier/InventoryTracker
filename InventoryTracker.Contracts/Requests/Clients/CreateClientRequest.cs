using InventoryTracker.Contracts.Requests.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Requests.Clients
{
    public class CreateClientRequest
    {
        public string Name { get; set; } = default!;
        public string ClientCode { get; set; } = default!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public CreateAddressRequest Address { get; set; } = default!;
    }
}
