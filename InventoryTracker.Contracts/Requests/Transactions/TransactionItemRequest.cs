using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Requests.Transactions
{
    public class TransactionItemRequest
    {
        public Guid ItemId { get; set; }
        public decimal Quantity { get; set; }
    }
}
