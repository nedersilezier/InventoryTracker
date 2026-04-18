namespace InventoryTracker.WebOperator.ViewModels.Transactions
{
    using InventoryTracker.Shared.Enums;

    public class OperatorTransactionsIndexViewModel
    {
        public string PageTitle { get; set; } = "Transactions";
        public string PageDescription { get; set; } = default!;

        public TransactionType? TypeFilter { get; set; }
        public TransactionStatus? StatusFilter { get; set; }

        public List<TransactionCardViewModel> Transactions { get; set; } = new();

        public PaginationViewModel Pagination { get; set; } = new();
    }
}
