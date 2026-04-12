using InventoryTracker.WebAdmin.ViewModels.HelperVMs;

namespace InventoryTracker.WebAdmin.ViewModels.Warehouses
{
    public class WarehousesIndexViewModel
    {
        public string? SearchTerm { get; set; }

        public List<WarehouseListItemViewModel> Warehouses { get; set; } = new();

        public int TotalCount { get; set; }

        public PaginationViewModel Pagination { get; set; } = new();
    }
}
