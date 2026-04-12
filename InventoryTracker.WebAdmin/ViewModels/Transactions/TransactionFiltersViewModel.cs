namespace InventoryTracker.WebAdmin.ViewModels.Transactions
{
    public class TransactionFiltersViewModel
    {
        public string? SearchTerm { get; set; }

        public string? DateRange { get; set; } = "30d";

        public bool IncludeInbound { get; set; } = true;
        public bool IncludeInternal { get; set; } = true;
        public bool IncludeOutbound { get; set; } = true;
    }
}
