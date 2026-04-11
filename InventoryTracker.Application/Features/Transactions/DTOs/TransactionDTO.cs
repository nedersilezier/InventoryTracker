using InventoryTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Transactions.DTOs
{
    public class TransactionDTO
    {
        public Guid TransactionId { get; set; }

        public TransactionType Type { get; set; }
        public string TypeName
        {
            get { return Type.ToString(); }

        }
        public TransactionStatus Status { get; set; }
        public string StatusName
        {
            get { return Status.ToString(); }
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

        public List<TransactionItemDTO> Items { get; set; } = new List<TransactionItemDTO>();
    }
}
