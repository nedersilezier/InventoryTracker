using InventoryTracker.Shared.Enums;
using InventoryTracker.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Transactions
{
    public class TransactionsResponseDTO
    {
        public Guid TransactionId { get; set; }

        public TransactionType Type { get; set; }
        public string TypeName { get; set; } = default!;
        public TransactionStatus Status { get; set; }
        public string StatusName { get; set; } = default!;

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
        public string TypeDisplay
        {
            get
            {
                return TransactionTypeExtensions.ToDisplayName(Type);
            }
        }
    }
}
