using InventoryTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Transactions
{
    public class TransactionListDTO
    {
        public Guid TransactionId { get; set; }

        public TransactionType Type { get; set; }
        public string TypeName
        {
            get
            {
                switch(this.Type)
                {
                    case TransactionType.Adjustment:
                        return "Adjustment";
                    case TransactionType.TransferBetweenWarehouses:
                        return "Transfer";
                    case TransactionType.IssueToClient:
                        return "Issue";
                    case TransactionType.ReturnFromClient:
                        return "Return";
                    default:
                        return "Unknown";
                }
            }
        }
        public TransactionStatus Status { get; set; }
        public string StatusName
        {
            get
            {
                switch(this.Status)
                {
                    case TransactionStatus.Draft:
                        return "Draft";
                    case TransactionStatus.Approved:
                        return "Approved";
                    case TransactionStatus.Cancelled:
                        return "Cancelled";
                    default:
                        return "Unknown";
                }
            }
        }

        public Guid? ClientId { get; set; }
        public string? ClientName { get; set; }

        public Guid? SourceWarehouseId { get; set; }
        public string? SourceWarehouseNameSnapshot { get; set; }

        public Guid? DestinationWarehouseId { get; set; }
        public string? DestinationWarehouseNameSnapshot { get; set; }

        public DateTime TransactionDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }

        public DateTime? CancelledAt { get; set; }
        public string? CancelledBy { get; set; }
        public string? CancellationReason { get; set; }
        public string? FromDisplay { get; set; } = default!;
        public string? ToDisplay { get; set; } = default!;
        public string TypeDisplay { get; set; } = default!;
    }
}
