using InventoryTracker.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Clients.DTOs
{
    public class ClientDetailsDTO
    {
        public Guid ClientId { get; set; }
        public string Name { get; set; } = default!;
        public string ClientCode { get; set; } = default!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public decimal Saldo { get; set; }

        public AddressDTO Address { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = default!;
        public string CreatedBy { get; set; } = default!;
        public DateTime? UpdatedAt { get; set; } = default!;
        public string? UpdatedBy { get; set; } = default!;
        public DateTime? DeletedAt { get; set; } = default!;
        public string? DeletedBy { get; set; } = default!;
    }
}
