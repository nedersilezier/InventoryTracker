using InventoryTracker.WebAdmin.ViewModels.HelperVMs;

namespace InventoryTracker.WebAdmin.ViewModels.Transactions
{
    public class TransactionsIndexViewModel
    {
        public TransactionFiltersViewModel Filters { get; set; } = new();

        public List<TransactionListItemViewModel> Transactions { get; set; } = new();

        public int TotalCount { get; set; }
        public int DraftCount { get; set; }
        public TableFooterViewModel TableFooter { get; set; } = new();
    }
}
