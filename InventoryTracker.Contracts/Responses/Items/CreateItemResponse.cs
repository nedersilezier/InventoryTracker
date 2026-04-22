using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Items
{
    public class CreateItemResponse
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = default!;
        public string SKU { get; set; } = default!;
    }
}
