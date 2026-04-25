using InventoryTracker.Contracts.Responses.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Clients
{
    public class ClientDetailsResponseDTO
    {
        public Guid ClientId { get; set; }
        public string Name { get; set; } = default!;
        public string ClientCode { get; set; } = default!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public decimal Saldo { get; set; }
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; } = default!;
        public DateTime? UpdatedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public AddressResponseDTO Address { get; set; } = default!;
    }
}
