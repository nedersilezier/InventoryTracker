using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Warehouses;

namespace InventoryTracker.WebAdmin.ViewModels.Stocks
{
    public class StocksIndexViewModel
    {
        public string? SearchTerm { get; set; }

        public List<StockListItemViewModel> Stocks { get; set; } = new();

        public int TotalCount { get; set; }

        public PaginationViewModel Pagination { get; set; } = new();
    }
}
