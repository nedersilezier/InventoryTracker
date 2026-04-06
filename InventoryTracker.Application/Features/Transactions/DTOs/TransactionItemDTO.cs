using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Transactions.DTOs
{
    public class TransactionItemDTO
    {
        public Guid TransactionItemId { get; set; }
        public Guid ItemId { get; set; }
        public string NameSnapshot { get; set; } = default!;
        public string SKUSnapshot { get; set; } = default!;
        public string UnitOfMeasureSnapshot { get; set; } = default!;
        public decimal Quantity { get; set; }
        public decimal UnitCreditValueSnapshot { get; set; }
        public decimal TotalCreditValue { get; set; }
    }
}
