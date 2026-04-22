using InventoryTracker.WebAdmin.ViewModels.HelperVMs;

namespace InventoryTracker.WebAdmin.ViewModels.Warehouses
{
    public class WarehousesIndexViewModel
    {
        public string? SearchTerm { get; set; }
        public int PageSize { get; set; }

        public List<WarehouseListItemViewModel> Warehouses { get; set; } = new();

        public TableFooterViewModel TableFooter { get; set; } = new();
    }
}
