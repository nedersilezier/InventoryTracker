using InventoryTracker.Shared.Enums;

namespace InventoryTracker.Contracts.Responses.Transactions
{
    public class TransactionForEditDTO
    {
        public Guid TransactionId { get; set; }

        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }

        public Guid? ClientId { get; set; }

        public Guid? SourceWarehouseId { get; set; }

        public Guid? DestinationWarehouseId { get; set; }

        public DateTime TransactionDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }

        public List<TransactionItemDTO> Items { get; set; } = new List<TransactionItemDTO>();
        public class TransactionItemDTO
        {
            public Guid ItemId { get; set; }
            public string NameSnapshot { get; set; } = default!;
            public string UnitOfMeasureSnapshot { get; set; } = default!;
            public decimal Quantity { get; set; }
        }
    }
}
