using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Items
{
    public class ItemResponseSelectDTO
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = default!;
        public string UnitOfMeasure { get; set; } = default!;
    }
}
