using InventoryTracker.Domain.Enums;

namespace InventoryTracker.API.Requests.Transactions
{
    public class UpdateTransactionRequest
    {
        public TransactionType Type { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? SourceWarehouseId { get; set; }
        public Guid? DestinationWarehouseId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public List<UpdateTransactionItemRequest> Items { get; set; } = new();

        public class UpdateTransactionItemRequest
        {
            public Guid ItemId { get; set; }
            public decimal Quantity { get; set; }
        }
    }
}
