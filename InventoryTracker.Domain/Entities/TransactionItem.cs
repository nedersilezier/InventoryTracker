using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Domain.Entities
{
    public class TransactionItem
    {
        public Guid TransactionItemId { get; set; }

        public Guid TransactionId { get; set; }
        public Transaction Transaction { get; set; } = default!;

        public Guid ReturnableItemId { get; set; }
        public Item Item { get; set; } = default!;

        public decimal Quantity { get; set; }

        public decimal UnitCreditValue { get; set; }
        public decimal TotalCreditValue { get; set; }
    }
}
