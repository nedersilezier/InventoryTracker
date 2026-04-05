using InventoryTracker.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Domain.Entities
{
    public class Item : SoftDeletableEntity
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = default!;
        public string SKU { get; set; } = default!;
        public string? Description { get; set; }
        public string UnitOfMeasure { get; set; } = "pcs";
        public decimal CreditValue { get; set; }
        public decimal Weight { get; set; }
        public bool IsActive { get; set; }
        public ICollection<TransactionItem> TransactionItems { get; set; } = new List<TransactionItem>();
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
}
