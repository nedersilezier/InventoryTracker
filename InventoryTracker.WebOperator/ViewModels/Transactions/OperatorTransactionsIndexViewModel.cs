namespace InventoryTracker.WebOperator.ViewModels.Transactions
{
    using InventoryTracker.Shared.Enums;

    public class OperatorTransactionsIndexViewModel
    {
        public string PageTitle { get; set; } = "Transactions";
        public int TotalCount { get; set; }
        public int DisplayedCount { get; set; }
        public List<TransactionCardViewModel> Transactions { get; set; } = new();

        public PaginationViewModel Pagination { get; set; } = new();
        public TransactionFiltersViewModel Filters { get; set; } = new();
    }
}
