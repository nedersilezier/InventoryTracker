namespace InventoryTracker.WebOperator.ViewModels.Transactions
{
    public class TransactionFiltersViewModel
    {
        public string? SearchTerm { get; set; }
        public DateTime? DateFrom { get; set; } = DateTime.Now;
        public DateTime? DateTo {  get; set; } = DateTime.Now;
        public bool IncludeAdjustments { get; set; } = true;
        public bool IncludeReturns { get; set; } = true;
        public bool IncludeIssues { get; set; } = true;
        public bool IncludeTransfers { get; set; } = true;
        public int? PageSize { get; set; }
    }
}
