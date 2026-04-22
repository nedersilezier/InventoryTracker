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

        [HttpGet]
        public IActionResult Create()
        {
            var createItemViewModel = new CreateItemViewModel();
            return View(createItemViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateItemViewModel vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var request = new CreateItemRequest
            {
                Name = vm.Name,
                SKU = vm.SKU,
                Description = vm.Description,
                UnitOfMeasure = vm.UnitOfMeasure,
                CreditValue = vm.CreditValue,
                Weight = vm.Weight
            };

            var result = await _itemsService.CreateItemAsync(request, cancellationToken);

            if (!result.Success)
            {
                if(result.ValidationErrors != null)
                {
                    foreach (var kvp in result.ValidationErrors)
                    {
                        foreach (var error in kvp.Value)
                        {
                            ModelState.AddModelError(kvp.Key, error);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Unable to create item.");
                }
                return View(vm);
            }
            TempData["SuccessMessage"] = $"Item '{result?.Data?.Name}' created successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
