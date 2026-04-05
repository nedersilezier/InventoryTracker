using InventoryTracker.Domain.Common;
using InventoryTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Domain.Entities
{
    public class Transaction : AuditableEntity
    {
        public Guid TransactionId { get; set; }

        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }

        public Guid? ClientId { get; set; }
        public Client? Client { get; set; }

        public Guid? SourceWarehouseId { get; set; }
        public Warehouse? SourceWarehouse { get; set; }

        public Guid? DestinationWarehouseId { get; set; }
        public Warehouse? DestinationWarehouse { get; set; }

        public string? SourceWarehouseNameSnapshot { get; set; }
        public string? DestinationWarehouseNameSnapshot { get; set; }
        public DateTime TransactionDate { get; set; }

        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }

        public DateTime? CancelledAt { get; set; }
        public string? CancelledBy { get; set; }
        public string? CancellationReason { get; set; }

        public ICollection<TransactionItem> TransactionItems { get; set; } = new List<TransactionItem>();
    }
}
