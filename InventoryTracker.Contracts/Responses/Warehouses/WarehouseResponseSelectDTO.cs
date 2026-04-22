using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Warehouses
{
    public class WarehouseResponseSelectDTO
    {
        public Guid WarehouseId { get; set; }
        public string Name { get; set; } = default!;
    }
}
