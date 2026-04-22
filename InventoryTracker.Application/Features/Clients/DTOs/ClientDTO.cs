using InventoryTracker.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Clients.DTOs
{
    public class ClientDTO
    {
        public Guid ClientId { get; set; }
        public string Name { get; set; } = default!;
        public string ClientCode { get; set; } = default!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public decimal Saldo { get; set; }

        public AddressDTO Address { get; set; } = default!;
    }
}
