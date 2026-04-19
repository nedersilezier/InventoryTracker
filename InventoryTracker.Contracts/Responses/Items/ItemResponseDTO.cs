using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Items
{
    public class ItemResponseDTO
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = default!;
        public string SKU { get; set; } = default!;
        public string? Description { get; set; }
        public string UnitOfMeasure { get; set; } = "pcs";
        public decimal CreditValue { get; set; }
        public decimal Weight { get; set; }
        public bool IsActive { get; set; }
    }
}
