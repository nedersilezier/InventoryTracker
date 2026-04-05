using InventoryTracker.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Domain.Entities
{
    public class Warehouse : SoftDeletableEntity
    {
        public Guid WarehouseId { get; set; }

        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;

        public Guid AddressId { get; set; }
        public Address Address { get; set; } = default!;

        public bool IsActive { get; set; } = true;
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
}
