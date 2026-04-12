using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Items;
using InventoryTracker.WebAdmin.ViewModels.Warehouses;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class WarehousesController : Controller
    {
        public IActionResult Index(int page = 1, string? searchTerm = null, int pageLenght = 10)
        {
            var testWarehouses = new List<WarehouseListItemViewModel>
                {
                    new WarehouseListItemViewModel { WarehouseId = Guid.NewGuid(), Name = "Warehouse 1", Code = "WH001", FullAddress = "123 Main St", City = "City A", CountryName = "Country X", StockEntriesCount = 10 },
                    new WarehouseListItemViewModel { WarehouseId = Guid.NewGuid(), Name = "Warehouse 2", Code = "WH002", FullAddress = "123 Main St", City = "City B", CountryName = "Country Y", StockEntriesCount = 20 },
                    new WarehouseListItemViewModel { WarehouseId = Guid.NewGuid(), Name = "Warehouse 3", Code = "WH003", FullAddress = "123 Main St", City = "City C", CountryName = "Country Z", StockEntriesCount = 30 },
                    new WarehouseListItemViewModel { WarehouseId = Guid.NewGuid(), Name = "Warehouse 4", Code = "WH001", FullAddress = "123 Main St", City = "City A", CountryName = "Country X", StockEntriesCount = 10 },
                    new WarehouseListItemViewModel { WarehouseId = Guid.NewGuid(), Name = "Warehouse 5", Code = "WH002", FullAddress = "123 Main St", City = "City B", CountryName = "Country Y", StockEntriesCount = 20 },
                    new WarehouseListItemViewModel { WarehouseId = Guid.NewGuid(), Name = "Warehouse 6", Code = "WH003", FullAddress = "123 Main St", City = "City C", CountryName = "Country Z", StockEntriesCount = 30 },
                    new WarehouseListItemViewModel { WarehouseId = Guid.NewGuid(), Name = "Warehouse 7", Code = "WH001", FullAddress = "123 Main St", City = "City A", CountryName = "Country X", StockEntriesCount = 10 },
                    new WarehouseListItemViewModel { WarehouseId = Guid.NewGuid(), Name = "Warehouse 8", Code = "WH001", FullAddress = "123 Main St", City = "City A", CountryName = "Country X", StockEntriesCount = 10 },
                    new WarehouseListItemViewModel { WarehouseId = Guid.NewGuid(), Name = "Warehouse 9", Code = "WH001", FullAddress = "123 Main St", City = "City A", CountryName = "Country X", StockEntriesCount = 10 },
                    new WarehouseListItemViewModel { WarehouseId = Guid.NewGuid(), Name = "Warehouse 10", Code = "WH001", FullAddress = "123 Main St", City = "City A", CountryName = "Country X", StockEntriesCount = 10 },
                    new WarehouseListItemViewModel { WarehouseId = Guid.NewGuid(), Name = "Warehouse 11", Code = "WH001", FullAddress = "123 Main St", City = "City A", CountryName = "Country X", StockEntriesCount = 10 },
                    new WarehouseListItemViewModel { WarehouseId = Guid.NewGuid(), Name = "Warehouse 12", Code = "WH001", FullAddress = "123 Main St", City = "City A", CountryName = "Country X", StockEntriesCount = 10 },
                };

            var testQuery = testWarehouses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                testQuery = testQuery.Where(item => item.Name.Contains(searchTerm) || item.Code.Contains(searchTerm) || item.City.Contains(searchTerm) || item.CountryName.Contains(searchTerm));

            var testViewModel = new WarehousesIndexViewModel
            {
                SearchTerm = searchTerm,
                Pagination = new PaginationViewModel
                {
                    CurrentPage = page,
                    Controller = "Warehouses",
                    TotalPages = (int)Math.Ceiling((double)testWarehouses.Count / pageLenght),
                    RouteValues = new Dictionary<string, string?> { { "searchTerm", searchTerm } }
                }
            };
            testViewModel.Warehouses = new List<WarehouseListItemViewModel>(testQuery.Skip((page - 1) * pageLenght).Take(pageLenght).ToList());
            return View(testViewModel);
        }
    }
}
