using InventoryTracker.Contracts.Requests.Clients;
using InventoryTracker.Contracts.Requests.Common;
using InventoryTracker.Contracts.Requests.Countries;
using InventoryTracker.Contracts.Requests.Items;
using InventoryTracker.WebAdmin.Filters;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.Services;
using InventoryTracker.WebAdmin.ViewModels.Clients;
using InventoryTracker.WebAdmin.ViewModels.Countries;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Items;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    [RequireAuth]
    public class CountriesController: BaseController
    {
        private readonly ICountriesService _countriesService;
        public CountriesController(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(GetCountriesRequest request, CancellationToken cancellationToken)
        {
            var result = await _countriesService.GetAllAsync(request, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to load countries.";

                return View(new CountriesIndexViewModel
                {
                    Countries = new List<CountryListItemViewModel>(),
                    SearchTerm = request.SearchTerm,
                    PageSize = request.PageSize ?? 5,
                    TableFooter = new TableFooterViewModel
                    {
                        DisplayedCount = 0,
                        TotalCount = 0,
                        EntityName = "countries",
                        Pagination = new PaginationViewModel
                        {
                            CurrentPage = request.PageNumber,
                            TotalPages = 1,
                            PageSize = request.PageSize ?? 5,
                            Controller = "Countries",
                            Action = "Index"
                        }
                    }
                });
            }
            return View(result.Data);
        }
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var viewModel = new CreateEditCountryViewModel();
            return View("CreateEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditCountryViewModel vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View("CreateEdit", vm);

            var request = new CreateCountryRequest
            {
                Name = vm.Name,
                Code = vm.CountryCode
            };

            var result = await _countriesService.CreateCountryAsync(request, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                AddServiceErrorsToModelState(result, "Unable to create country");
                return View("CreateEdit", vm);
            }
            TempData["SuccessMessage"] = $"Country '{result?.Data?.Name}' created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
        {
            var result = await _countriesService.GetByIdAsync(id, cancellationToken);
            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to load country.";
                return RedirectToAction(nameof(Index));
            }
            return View("CreateEdit", result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateEditCountryViewModel vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View("CreateEdit", vm);

            var request = new UpdateCountryRequest
            {
                Name = vm.Name,
                Code = vm.CountryCode
            };
            var id = vm.CountryId.GetValueOrDefault();
            var result = await _countriesService.UpdateCountryAsync(id, request, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                AddServiceErrorsToModelState(result, "Unable to update the country");
                return View("CreateEdit", vm);
            }
            TempData["SuccessMessage"] = $"Country '{result?.Data?.Name}' updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _countriesService.DeleteCountryAsync(id, cancellationToken);
            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to delete country.";
            }
            else
            {
                TempData["SuccessMessage"] = $"Country deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
