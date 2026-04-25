using InventoryTracker.Contracts.Requests.Items;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Items;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class ItemsController : BaseController
    {
        private readonly IItemsService _itemsService;
        public ItemsController(IItemsService itemsService)
        {
            _itemsService = itemsService;
        }
        public async Task<IActionResult> Index(GetItemsRequest request, CancellationToken cancellationToken)
        {
            var result = await _itemsService.GetAllAsync(request, cancellationToken);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to load items.";

                return View(new ItemsIndexViewModel
                {
                    Items = new List<ItemListItemViewModel>(),
                    SearchTerm = request.SearchTerm,
                    PageSize = request.PageSize ?? 5,
                    TableFooter = new TableFooterViewModel
                    {
                        DisplayedCount = 0,
                        TotalCount = 0,
                        EntityName = "items",
                        Pagination = new PaginationViewModel
                        {
                            CurrentPage = request.PageNumber,
                            TotalPages = 1,
                            PageSize = request.PageSize ?? 5,
                            Controller = "Items",
                            Action = "Index"
                        }
                    }
                });
            }

            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
        {
            var result = await _itemsService.GetDetailsByIdAsync(id, cancellationToken);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to load item.";
                return RedirectToAction(nameof(Index));
            }
            return View("Details", result.Data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var createItemViewModel = new CreateEditItemViewModel();
            return View("CreateEdit", createItemViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditItemViewModel vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View("CreateEdit", vm);

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
                AddServiceErrorsToModelState(result, "Unable to create item");
                return View("CreateEdit", vm);
            }
            TempData["SuccessMessage"] = $"Item '{result?.Data?.Name}' created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
        {
            var result = await _itemsService.GetByIdAsync(id, cancellationToken);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to load item.";
                return RedirectToAction(nameof(Index));
            }
            return View("CreateEdit", result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateEditItemViewModel vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var request = new UpdateItemRequest
            {
                Name = vm.Name,
                SKU = vm.SKU,
                Description = vm.Description,
                UnitOfMeasure = vm.UnitOfMeasure,
                CreditValue = vm.CreditValue,
                Weight = vm.Weight
            };
            var id = vm.ItemId.GetValueOrDefault();
            var result = await _itemsService.UpdateItemAsync(id, request, cancellationToken);

            if (!result.Success)
            {
                AddServiceErrorsToModelState(result, "Unable to update the item");
                return View("CreateEdit", vm);
            }
            TempData["SuccessMessage"] = $"Item '{result?.Data?.Name}' updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(Guid id, string? returnUrl, CancellationToken cancellationToken)
        {
            var result = await _itemsService.DeactivateItemAsync(id, cancellationToken);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to deactivate item.";
            }
            else
            {
                TempData["SuccessMessage"] = $"Item '{result.Data!.Name}' deactivated successfully.";
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(Guid id, string? returnUrl, CancellationToken cancellationToken)
        {
            var result = await _itemsService.ActivateItemAsync(id, cancellationToken);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to activate item.";
            }
            else
            {
                TempData["SuccessMessage"] = $"Item '{result.Data!.Name}' activated successfully.";
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }
    }
}
