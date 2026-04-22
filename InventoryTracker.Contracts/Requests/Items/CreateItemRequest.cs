using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Requests.Items
{
    public class CreateItemRequest
    {
        public string Name { get; set; } = default!;
        public string SKU { get; set; } = default!;
        public string? Description { get; set; }
        public string UnitOfMeasure { get; set; } = default!;
        public decimal CreditValue { get; set; }
        public decimal Weight { get; set; }
    }
}
