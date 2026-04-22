using InventoryTracker.Contracts.Requests.Items;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Items;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IItemsService _itemsService;
        public ItemsController(IItemsService itemsService)
        {
            _itemsService = itemsService;
        }
        public async Task<IActionResult> Index(GetItemsRequest request, CancellationToken cancellationToken)
        {
            var itemsListViewModel = await _itemsService.GetAllAsync(request, cancellationToken);
            return View(itemsListViewModel);
        }
    }
}
