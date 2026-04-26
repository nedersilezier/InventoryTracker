using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Clients;
using InventoryTracker.Contracts.Requests.Common;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.Clients;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static InventoryTracker.Contracts.Requests.Clients.UpdateClientRequest;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class ClientsController : BaseController
    {
        private readonly IClientsService _clientsService;
        private readonly ILookupsService _lookupsService;
        public ClientsController(IClientsService clientsService, ILookupsService lookupsService)
        {
            _clientsService = clientsService;
            _lookupsService = lookupsService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(GetClientsRequest request, CancellationToken cancellationToken)
        {
            var result = await _clientsService.GetAllAsync(request, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to load clients.";

                return View(new ClientsIndexViewModel
                {
                    Clients = new List<ClientListItemViewModel>(),
                    SearchTerm = request.SearchTerm,
                    PageSize = request.PageSize ?? 5,
                    TableFooter = new TableFooterViewModel
                    {
                        DisplayedCount = 0,
                        TotalCount = 0,
                        EntityName = "clients",
                        Pagination = new PaginationViewModel
                        {
                            CurrentPage = request.PageNumber,
                            TotalPages = 1,
                            PageSize = request.PageSize ?? 5,
                            Controller = "Clients",
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
            var result = await _clientsService.GetDetailsByIdAsync(id, cancellationToken);
            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to load client.";
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

            var viewModel = new CreateEditClientViewModel
            {
                AvailableCountries = countriesResult.Data!
            };

            return View("CreateEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditClientViewModel vm, CancellationToken cancellationToken)
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
            
            var request = new CreateClientRequest
            {
                Name = vm.Name,
                ClientCode = vm.ClientCode,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber,
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

            var result = await _clientsService.CreateClientAsync(request, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                AddServiceErrorsToModelState(result, "Unable to create client");
                return View("CreateEdit", vm);
            }
            TempData["SuccessMessage"] = $"Client '{result?.Data?.ClientName}' created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
        {
            var result = await _clientsService.GetByIdAsync(id, cancellationToken);
            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to load client.";
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
        public async Task<IActionResult> Edit(CreateEditClientViewModel vm, CancellationToken cancellationToken)
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
            var request = new UpdateClientRequest
            {
                Name = vm.Name,
                ClientCode = vm.ClientCode,
                Email = vm.Email ?? string.Empty,
                PhoneNumber = vm.PhoneNumber ?? string.Empty,
                Address = new UpdateClientAddressRequest
                {
                    Street = vm.Address.Street,
                    HouseNumber = vm.Address.HouseNumber,
                    ApartmentNumber = vm.Address.ApartmentNumber,
                    PostalCode = vm.Address.PostalCode,
                    City = vm.Address.City,
                    CountryId = vm.Address.CountryId
                }
            };
            var id = vm.ClientId.GetValueOrDefault();
            var result = await _clientsService.UpdateClientAsync(id, request, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                AddServiceErrorsToModelState(result, "Unable to update the client");
                return View("CreateEdit", vm);
            }
            TempData["SuccessMessage"] = $"Client '{result?.Data?.ClientName}' updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(Guid id, string? returnUrl, CancellationToken cancellationToken)
        {
            var result = await _clientsService.DeactivateClientAsync(id, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to deactivate client.";
            }
            else
            {
                TempData["SuccessMessage"] = $"Client '{result.Data!.ClientName}' deactivated successfully.";
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(Guid id, string? returnUrl, CancellationToken cancellationToken)
        {
            var result = await _clientsService.ActivateClientAsync(id, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to activate client.";
            }
            else
            {
                TempData["SuccessMessage"] = $"Client '{result.Data!.ClientName}' activated successfully.";
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
