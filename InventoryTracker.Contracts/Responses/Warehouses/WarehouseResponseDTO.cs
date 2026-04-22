using InventoryTracker.Contracts.Responses.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Warehouses
{
    public class WarehouseResponseDTO
    {
        public Guid WarehouseId { get; set; }

        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
        public AddressResponseDTO Address { get; set; } = default!;
        public int StockCount { get; set; }
    }
}
