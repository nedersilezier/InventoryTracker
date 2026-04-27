using InventoryTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Requests.Transactions
{
    public class CreateTransactionRequest
    {
        public TransactionType Type { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? SourceWarehouseId { get; set; }
        public Guid? DestinationWarehouseId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public List<TransactionItemRequest> Items { get; set; } = new();
    }
}
