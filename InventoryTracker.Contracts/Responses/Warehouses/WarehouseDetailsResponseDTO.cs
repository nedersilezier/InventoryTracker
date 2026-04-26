using InventoryTracker.Contracts.Responses.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Warehouses
{
    public class WarehouseDetailsResponseDTO
    {
        public Guid WarehouseId { get; set; }

        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
        public AddressResponseDTO Address { get; set; } = default!;
        public int StocksCount { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; } = default!;
        public DateTime? UpdatedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
