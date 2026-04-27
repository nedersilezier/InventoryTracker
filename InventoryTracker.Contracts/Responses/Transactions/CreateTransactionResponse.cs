using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Transactions
{
    public class CreateTransactionResponse
    {
        public Guid TransactionId {  get; set; }
    }
}
