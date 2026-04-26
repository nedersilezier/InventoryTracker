using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Common;
using InventoryTracker.Contracts.Requests.Warehouses;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Items;
using InventoryTracker.WebAdmin.ViewModels.Warehouses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static InventoryTracker.Contracts.Requests.Warehouses.UpdateWarehouseRequest;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class WarehousesController : BaseController
    {
        private readonly IWarehousesService _warehousesService;
        private readonly ILookupsService _lookupsService;
        public WarehousesController(IWarehousesService warehousesService, ILookupsService lookupsService)
        {
            _warehousesService = warehousesService;
            _lookupsService = lookupsService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(GetWarehousesRequest request, CancellationToken cancellationToken)
        {
            var result = await _warehousesService.GetAllAsync(request, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to load warehouses.";

                return View(new WarehousesIndexViewModel
                {
                    Warehouses = new List<WarehouseListItemViewModel>(),
                    SearchTerm = request.SearchTerm,
                    PageSize = request.PageSize ?? 5,
                    TableFooter = new TableFooterViewModel
                    {
                        DisplayedCount = 0,
                        TotalCount = 0,
                        EntityName = "warehouses",
                        Pagination = new PaginationViewModel
                        {
                            CurrentPage = request.PageNumber,
                            TotalPages = 1,
                            PageSize = request.PageSize ?? 5,
                            Controller = "Warehouses",
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
            var result = await _warehousesService.GetDetailsByIdAsync(id, cancellationToken);
            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to load warehouse.";
                return RedirectToAction(nameof(Index));
            }
            return View("Details", result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var countriesResult = await GetCountrySelectListAsync(null, cancellationToken);

            if (!countriesResult.Success)
            {
                var authFailure = HandleAuthFailure(countriesResult);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = countriesResult.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new CreateEditWarehouseViewModel
            {
                AvailableCountries = countriesResult.Data!
            };

            return View("CreateEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditWarehouseViewModel vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                var countriesResult = await GetCountrySelectListAsync(null, cancellationToken);

                if (!countriesResult.Success)
                {
                    var authFailure = HandleAuthFailure(countriesResult);
                    if (authFailure is not null)
                        return authFailure;

                    TempData["ErrorMessage"] = countriesResult.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
                vm.AvailableCountries = countriesResult.Data!;
                return View("CreateEdit", vm);
            }

            var request = new CreateWarehouseRequest
            {
                Name = vm.Name,
                Code = vm.WarehouseCode,
                Address = new CreateAddressRequest
                {
                    Street = vm.Address.Street,
                    HouseNumber = vm.Address.HouseNumber,
                    ApartmentNumber = vm.Address.ApartmentNumber,
                    PostalCode = vm.Address.PostalCode,
                    City = vm.Address.City,
                    CountryId = vm.Address.CountryId
                }
            };

            var result = await _warehousesService.CreateWarehouseAsync(request, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                AddServiceErrorsToModelState(result, "Unable to create warehouse");
                return View("CreateEdit", vm);
            }
            TempData["SuccessMessage"] = $"Warehouse '{result?.Data?.Name}' created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
        {
            var result = await _warehousesService.GetByIdAsync(id, cancellationToken);
            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to load warehouse.";
                return RedirectToAction(nameof(Index));
            }
            var countriesResult = await GetCountrySelectListAsync(null, cancellationToken);

            if (!countriesResult.Success)
            {
                var authFailure = HandleAuthFailure(countriesResult);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = countriesResult.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            result.Data!.AvailableCountries = countriesResult.Data!;
            return View("CreateEdit", result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateEditWarehouseViewModel vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                var countriesResult = await GetCountrySelectListAsync(null, cancellationToken);

                if (!countriesResult.Success)
                {
                    var authFailure = HandleAuthFailure(countriesResult);
                    if (authFailure is not null)
                        return authFailure;

                    TempData["ErrorMessage"] = countriesResult.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }

                vm.AvailableCountries = countriesResult.Data!;
                return View("CreateEdit", vm);
            }
            var request = new UpdateWarehouseRequest
            {
                Name = vm.Name,
                Code = vm.WarehouseCode,
                Address = new UpdateWarehouseAddressRequest
                {
                    Street = vm.Address.Street,
                    HouseNumber = vm.Address.HouseNumber,
                    ApartmentNumber = vm.Address.ApartmentNumber,
                    PostalCode = vm.Address.PostalCode,
                    City = vm.Address.City,
                    CountryId = vm.Address.CountryId
                }
            };
            var id = vm.WarehouseId.GetValueOrDefault();
            var result = await _warehousesService.UpdateWarehouseAsync(id, request, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                AddServiceErrorsToModelState(result, "Unable to update the warehouse");
                return View("CreateEdit", vm);
            }
            TempData["SuccessMessage"] = $"Warehouse '{result?.Data?.Name}' updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(Guid id, string? returnUrl, CancellationToken cancellationToken)
        {
            var result = await _warehousesService.DeactivateWarehouseAsync(id, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to deactivate warehouse.";
            }
            else
            {
                TempData["SuccessMessage"] = $"Warehouse '{result.Data!.Name}' deactivated successfully.";
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(Guid id, string? returnUrl, CancellationToken cancellationToken)
        {
            var result = await _warehousesService.ActivateWarehouseAsync(id, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to activate warehouse.";
            }
            else
            {
                TempData["SuccessMessage"] = $"Warehouse '{result.Data!.Name}' activated successfully.";
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }
        #region Helpers
        private async Task<ServiceResult<List<SelectListItem>>> GetCountrySelectListAsync(Guid? selectedCountryId, CancellationToken cancellationToken)
        {
            var countriesResult = await _lookupsService.GetCountriesAsync(cancellationToken);

            if (!countriesResult.Success)
                return ServiceResult<List<SelectListItem>>.Fail(countriesResult.ErrorMessage ?? "Unable to load countries.", countriesResult.ValidationErrors, countriesResult.StatusCode);

            var items = countriesResult.Data!
                .Select(c => new SelectListItem
                {
                    Value = c.CountryId.ToString(),
                    Text = c.CountryName,
                    Selected = selectedCountryId.HasValue && c.CountryId == selectedCountryId.Value
                })
                .ToList();

            return ServiceResult<List<SelectListItem>>.Ok(items);
        }
        #endregion

    }
}
