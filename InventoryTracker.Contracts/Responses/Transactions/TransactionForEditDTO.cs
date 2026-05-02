using InventoryTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Transactions
{
    public class TransactionForEditDTO
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

        public List<TransactionItemDTO> Items { get; set; } = new List<TransactionItemDTO>();
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
}
