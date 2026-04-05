using InventoryTracker.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Domain.Entities
{
    public class Stock : AuditableEntity
    {
        public Guid StockId { get; set; }

        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } = default!;

        public Guid ItemId { get; set; }
        public Item Item { get; set; } = default!;

        public decimal Quantity { get; set; }
    }
}
