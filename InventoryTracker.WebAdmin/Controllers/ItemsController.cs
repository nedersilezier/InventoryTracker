using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Items;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class ItemsController : Controller
    {
        public IActionResult Index(int page = 1, string? searchTerm = null, int pageLenght = 10)
        {
            var testItems = new List<ItemListItemViewModel>
                {
                    new ItemListItemViewModel { ItemId = Guid.NewGuid(), Name = "Item 1", SKU = "SKU001", UnitOfMeasure = "pcs", CreditValue = 10.00m, Weight = 1.5m },
                    new ItemListItemViewModel { ItemId = Guid.NewGuid(), Name = "Item 2", SKU = "SKU002", UnitOfMeasure = "pcs", CreditValue = 20.00m, Weight = 2.0m },
                    new ItemListItemViewModel { ItemId = Guid.NewGuid(), Name = "Item 3", SKU = "SKU003", UnitOfMeasure = "pcs", CreditValue = 15.00m, Weight = 1.0m },
                    new ItemListItemViewModel { ItemId = Guid.NewGuid(), Name = "Item 4", SKU = "SKU004", UnitOfMeasure = "pcs", CreditValue = 25.00m, Weight = 3.0m },
                    new ItemListItemViewModel { ItemId = Guid.NewGuid(), Name = "Item 5", SKU = "SKU005", UnitOfMeasure = "pcs", CreditValue = 30.00m, Weight = 4.0m },
                    new ItemListItemViewModel { ItemId = Guid.NewGuid(), Name = "Item 6", SKU = "SKU006", UnitOfMeasure = "pcs", CreditValue = 12.00m, Weight = 1.2m },
                    new ItemListItemViewModel{ ItemId = Guid.NewGuid(), Name = "Item 7", SKU = "SKU007", UnitOfMeasure = "pcs", CreditValue = 18.00m, Weight = 2.5m  },
                    new ItemListItemViewModel { ItemId = Guid.NewGuid(), Name = "Item 8", SKU = "SKU008", UnitOfMeasure = "pcs", CreditValue = 22.00m, Weight = 3.5m },
                    new ItemListItemViewModel { ItemId = Guid.NewGuid(), Name = "Item 9", SKU = "SKU009", UnitOfMeasure = "pcs", CreditValue = 28.00m, Weight = 4.5m },
                    new ItemListItemViewModel { ItemId = Guid.NewGuid(), Name = "Item 10", SKU = "SKU010", UnitOfMeasure = "pcs", CreditValue = 35.00m, Weight = 5.0m },
                    new ItemListItemViewModel { ItemId = Guid.NewGuid(), Name = "Item 11", SKU = "SKU011", UnitOfMeasure = "pcs", CreditValue = 40.00m, Weight = 6.0m }
                };
            var testQuery = testItems.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                testQuery = testQuery.Where(item => item.Name.Contains(searchTerm) || item.SKU.Contains(searchTerm) || (item.Description != null && item.Description.Contains(searchTerm)));

            var testViewModel = new ItemsIndexViewModel
            {
                SearchTerm = searchTerm,
                Pagination = new PaginationViewModel
                {
                    CurrentPage = page,
                    Controller = "Items",
                    TotalPages = 2,
                    RouteValues = new Dictionary<string, string?> { { "searchTerm", searchTerm } }
                }
            };
            testViewModel.Items = new List<ItemListItemViewModel>(testQuery.Skip((page - 1) * pageLenght).Take(pageLenght).ToList());
            return View(testViewModel);
        }
    }
}
