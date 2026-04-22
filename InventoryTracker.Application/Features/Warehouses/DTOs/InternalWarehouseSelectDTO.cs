using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Warehouses.DTOs
{
    public class InternalWarehouseSelectDTO
    {
        public Guid WarehouseId { get; set; }
        public string Name { get; set; } = default!;
    }
}
