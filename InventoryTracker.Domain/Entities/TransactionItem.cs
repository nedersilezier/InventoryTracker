using InventoryTracker.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Domain.Entities
{
    public class TransactionItem: AuditableEntity
    {
        public Guid TransactionItemId { get; set; }

        public Guid TransactionId { get; set; }
        public Transaction Transaction { get; set; } = default!;

        public Guid ItemId { get; set; }
        public Item Item { get; set; } = default!;

        public string NameSnapshot { get; set; } = default!;
        public string SKUSnapshot { get; set; } = default!;
        public string UnitOfMeasureSnapshot { get; set; } = default!;
        public decimal Quantity { get; set; }
        public decimal UnitCreditValueSnapshot { get; set; }
        public decimal TotalCreditValue { get; private set; }
    }
}
