using InventoryTracker.Shared.Enums;

namespace InventoryTracker.WebAdmin.ViewModels.Transactions
{
    public class TransactionListItemViewModel
    {
        public Guid TransactionId { get; set; }
        public string? ReferenceNumber { get; set; }
        public decimal Quantity { get; set; }
        public string QuantityDisplay { get; set; } = default!;

        public decimal TotalCreditValue { get; set; }
        public string TotalCreditValueDisplay { get; set; } = default!;

        public string FromDisplay { get; set; } = default!;
        public string ToDisplay { get; set; } = default!;

        public DateTime TransactionDate { get; set; }
        public TransactionType Type { get; set; }
        public string TypeDisplay { get; set; } = default!;
        public TransactionStatus Status { get; set; } = default!;
        public string StatusDisplay { get; set; } = default!;
    }
}
