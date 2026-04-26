using InventoryTracker.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Warehouses.DTOs
{
    public class WarehouseDetailsDTO
    {
        public Guid WarehouseId { get; set; }

        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
        public AddressDTO Address { get; set; } = default!;
        public int StocksCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = default!;
        public string CreatedBy { get; set; } = default!;
        public DateTime? UpdatedAt { get; set; } = default!;
        public string? UpdatedBy { get; set; } = default!;
        public DateTime? DeletedAt { get; set; } = default!;
        public string? DeletedBy { get; set; } = default!;
    }
}
