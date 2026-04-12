using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Stocks;
using InventoryTracker.WebAdmin.ViewModels.Warehouses;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class StocksController : Controller
    {
        public IActionResult Index(int page = 1, string? searchTerm = null, int pageLenght = 10)
        {
            var testStocks = new List<StockListItemViewModel>
                {
                    new StockListItemViewModel { WarehouseId = Guid.NewGuid(), WarehouseName = "Warehouse 1", ItemId = Guid.NewGuid(), ItemName = "Item 1", Quantity = 100 },
                    new StockListItemViewModel { WarehouseId = Guid.NewGuid(), WarehouseName = "Warehouse 2", ItemId = Guid.NewGuid(), ItemName = "Item 2", Quantity = 200 },
                    new StockListItemViewModel { WarehouseId = Guid.NewGuid(), WarehouseName = "Warehouse 3", ItemId = Guid.NewGuid(), ItemName = "Item 3", Quantity = 300 },
                    new StockListItemViewModel { WarehouseId = Guid.NewGuid(), WarehouseName = "Warehouse 4", ItemId = Guid.NewGuid(), ItemName = "Item 4", Quantity = 400 },
                    new StockListItemViewModel { WarehouseId = Guid.NewGuid(), WarehouseName = "Warehouse 5", ItemId = Guid.NewGuid(), ItemName = "Item 5", Quantity = 500 },
                    new StockListItemViewModel { WarehouseId = Guid.NewGuid(), WarehouseName = "Warehouse 6", ItemId = Guid.NewGuid(), ItemName = "Item 6", Quantity = 600 },
                    new StockListItemViewModel { WarehouseId = Guid.NewGuid(), WarehouseName = "Warehouse 7", ItemId = Guid.NewGuid(), ItemName = "Item 7", Quantity = 700 },
                    new StockListItemViewModel { WarehouseId = Guid.NewGuid(), WarehouseName = "Warehouse 8", ItemId = Guid.NewGuid(), ItemName = "Item 8", Quantity = 800 },
                    new StockListItemViewModel { WarehouseId = Guid.NewGuid(), WarehouseName = "Warehouse 9", ItemId = Guid.NewGuid(), ItemName = "Item 9", Quantity = 900 },
                    new StockListItemViewModel { WarehouseId = Guid.NewGuid(), WarehouseName = "Warehouse 10", ItemId = Guid.NewGuid(), ItemName = "Item 10", Quantity = 1000 },
                    new StockListItemViewModel { WarehouseId = Guid.NewGuid(), WarehouseName = "Warehouse 10", ItemId = Guid.NewGuid(), ItemName = "Item 9", Quantity = 200 },
                    new StockListItemViewModel { WarehouseId = Guid.NewGuid(), WarehouseName = "Warehouse 10", ItemId = Guid.NewGuid(), ItemName = "Item 8", Quantity = 444 },
                };

            var testQuery = testStocks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                testQuery = testQuery.Where(stock => stock.ItemName.Contains(searchTerm) || stock.WarehouseName.Contains(searchTerm));

            var testViewModel = new StocksIndexViewModel
            {
                TotalCount = testStocks.Count,
                SearchTerm = searchTerm,
                Pagination = new PaginationViewModel
                {
                    CurrentPage = page,
                    Controller = "Stocks",
                    TotalPages = (int)Math.Ceiling((double)testStocks.Count / pageLenght),
                    RouteValues = new Dictionary<string, string?> { { "searchTerm", searchTerm } }
                }
            };
            testViewModel.Stocks = new List<StockListItemViewModel>(testQuery.Skip((page - 1) * pageLenght).Take(pageLenght).ToList());
            return View(testViewModel);
        }
    }
}
