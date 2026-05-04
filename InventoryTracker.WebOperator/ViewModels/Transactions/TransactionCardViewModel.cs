namespace InventoryTracker.WebOperator.ViewModels.Transactions
{
    using InventoryTracker.Shared.Enums;
    using InventoryTracker.Shared.Extensions;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    public class TransactionCardViewModel
    {
        public Guid TransactionId { get; set; }

        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }
        public string TypeDisplay
        {
            get
            {
                return TransactionTypeExtensions.ToDisplayName(Type);
            }
        }
        public string StatusDisplay
        {
            get
            {
                switch(Status)
                {
                    case TransactionStatus.Approved:
                        return "Approved";
                    case TransactionStatus.Draft:
                        return "Draft";
                    case TransactionStatus.Cancelled:
                        return "Cancelled";
                    default:
                        return "Unknown";
                }
            }
        }
        public string FromDisplay { get; set; } = default!;
        public string ToDisplay { get; set; } = default!;
        public string ReferenceNumber { get; set; } = default!;
        public DateTime TransactionDate { get; set; }
        public string ContinueAction { get; set; } = "Edit";
        public string ContinueController { get; set; } = "Transactions";
    }
}
