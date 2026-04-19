namespace InventoryTracker.WebAdmin.ViewModels.HelperVMs
{
    public class TableFooterViewModel
    {
        public int DisplayedCount { get; set; }
        public int TotalCount { get; set; }
        public string EntityName { get; set; } = "";
        public PaginationViewModel Pagination { get; set; } = new();
    }
}
