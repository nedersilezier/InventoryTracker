using InventoryTracker.WebAdmin.ViewModels.HelperVMs;

namespace InventoryTracker.WebAdmin.ViewModels.Transactions
{
    public class TransactionsIndexViewModel
    {
        public TransactionFiltersViewModel Filters { get; set; } = new();

        public List<TransactionListItemViewModel> Transactions { get; set; } = new();

        public int TotalCount { get; set; }

        public int InboundCount { get; set; }
        public int InternalCount { get; set; }
        public int OutboundCount { get; set; }

        public decimal TotalCreditValue { get; set; }

        public PaginationViewModel Pagination { get; set; } = new();
    }
}
