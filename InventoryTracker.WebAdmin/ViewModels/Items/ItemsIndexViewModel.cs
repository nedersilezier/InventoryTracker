using InventoryTracker.WebAdmin.ViewModels.HelperVMs;

namespace InventoryTracker.WebAdmin.ViewModels.Items
{
    public class ItemsIndexViewModel
    {
        public string? SearchTerm { get; set; }

        public List<ItemListItemViewModel> Items { get; set; } = new();

        public int TotalCount
        {
            get
            {
                return Items.Count;
            }
        }

        public PaginationViewModel Pagination { get; set; } = new();
    }
}
