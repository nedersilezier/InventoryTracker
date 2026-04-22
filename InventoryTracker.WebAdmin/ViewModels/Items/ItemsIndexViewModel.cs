using InventoryTracker.WebAdmin.ViewModels.HelperVMs;

namespace InventoryTracker.WebAdmin.ViewModels.Items
{
    public class ItemsIndexViewModel
    {
        public string? SearchTerm { get; set; }

        public List<ItemListItemViewModel> Items { get; set; } = new();

        public int TotalCount { get; set; }
        public int TotalActiveItems { get; set; }
        public int PageSize { get; set; }

        public TableFooterViewModel TableFooter { get; set; } = new();
    }
}
