using InventoryTracker.Contracts.Responses.Warehouses;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Warehouses;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryTracker.WebAdmin.ViewModels.Stocks
{
    public class StocksIndexViewModel
    {
        public string? SearchTerm { get; set; }
        public Guid? SelectedWarehouseId { get; set; }

        public List<StockListItemViewModel> Stocks { get; set; } = new();
        public List<SelectListItem> Warehouses { get; set; } = new();

        public int TotalCount { get; set; }

        public TableFooterViewModel TableFooter { get; set; } = new();
        public int? PageSize { get; set; }
    }
}
