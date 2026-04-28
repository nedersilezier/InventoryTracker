using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Transactions;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Items;
using InventoryTracker.WebAdmin.ViewModels.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class TransactionsController : BaseController
    {
        private readonly ITransactionsService _transactionsService;
        private readonly ILookupsService _lookupsService;
        public TransactionsController(ITransactionsService transactionsService, ILookupsService lookupsService)
        {
            _transactionsService = transactionsService;
            _lookupsService = lookupsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(GetTransactionsRequest request, CancellationToken cancellationToken)
        {
            if (!request.DateFrom.HasValue && !request.DateTo.HasValue)
            {
                var today = DateTime.Today;
                request.DateFrom = today;
                request.DateTo = today;
            }
            var result = await _transactionsService.GetAllAsync(request, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to load transactions.";

                return View(new TransactionsIndexViewModel
                {
                    TotalCount = 0,
                    DraftCount = 0,
                    Filters = new TransactionFiltersViewModel
                    {
                        SearchTerm = request.SearchTerm,
                        IncludeAdjustments = request.IncludeAdjustments,
                        IncludeTransfers = request.IncludeTransfers,
                        IncludeIssues = request.IncludeIssues,
                        IncludeReturns = request.IncludeReturns,
                        DateFrom = request.DateFrom,
                        DateTo = request.DateTo,
                    },
                    TableFooter = new TableFooterViewModel
                    {
                        DisplayedCount = 0,
                        TotalCount = 0,
                        EntityName = "transactions",
                        Pagination = new PaginationViewModel
                        {
                            CurrentPage = 1,
                            TotalPages = 1,
                            PageSize = request.PageSize,
                            Controller = "Transactions",
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
            var itemsResult = await GetItemSelectListAsync(cancellationToken);

            if (!itemsResult.Success)
            {
                var authFailure = HandleAuthFailure(itemsResult);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = itemsResult.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }
            var clientsResult = await GetClientSelectListAsync(null, cancellationToken);

            if (!clientsResult.Success)
            {
                var authFailure = HandleAuthFailure(clientsResult);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = clientsResult.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }
            var warehousesResult = await GetWarehouseSelectListAsync(null, cancellationToken);

            if (!warehousesResult.Success)
            {
                var authFailure = HandleAuthFailure(warehousesResult);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = warehousesResult.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new CreateTransactionViewModel
            {
                AvailableItems = itemsResult.Data!,
                AvailableWarehouses = warehousesResult.Data!,
                AvailableClients = clientsResult.Data!
            };
            return View("Create", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTransactionViewModel vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                var itemsResult = await GetItemSelectListAsync(cancellationToken);

                if (!itemsResult.Success)
                {
                    var authFailure = HandleAuthFailure(itemsResult);
                    if (authFailure is not null)
                        return authFailure;

                    TempData["ErrorMessage"] = itemsResult.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
                var clientsResult = await GetClientSelectListAsync(null, cancellationToken);

                if (!clientsResult.Success)
                {
                    var authFailure = HandleAuthFailure(clientsResult);
                    if (authFailure is not null)
                        return authFailure;

                    TempData["ErrorMessage"] = clientsResult.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
                var warehousesResult = await GetWarehouseSelectListAsync(null, cancellationToken);

                if (!warehousesResult.Success)
                {
                    var authFailure = HandleAuthFailure(warehousesResult);
                    if (authFailure is not null)
                        return authFailure;

                    TempData["ErrorMessage"] = warehousesResult.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
                vm.AvailableItems = itemsResult.Data!;
                vm.AvailableClients = clientsResult.Data!;
                vm.AvailableWarehouses = warehousesResult.Data!;

                return View("Create", vm);
            }
            var localDate = vm.TransactionDate.Date.Add(DateTime.Now.TimeOfDay);
            var utcDate = DateTime.SpecifyKind(localDate, DateTimeKind.Local).ToUniversalTime();

            var request = new CreateTransactionRequest
            {
                Type = vm.Type,
                TransactionDate = utcDate,
                ClientId = vm.ClientId,
                SourceWarehouseId = vm.SourceWarehouseId,
                DestinationWarehouseId = vm.DestinationWarehouseId,
                ReferenceNumber = vm.ReferenceNumber,
                Notes = vm.Notes,
                Items = vm.SelectedItems.Select(i => new TransactionItemRequest { ItemId = i.ItemId, Quantity = i.Quantity }).ToList()
            };

            var result = await _transactionsService.CreateTransactionAsync(request, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                AddServiceErrorsToModelState(result, "Unable to create transaction");
                var itemsResult = await GetItemSelectListAsync(cancellationToken);

                if (!itemsResult.Success)
                {
                    authFailure = HandleAuthFailure(itemsResult);
                    if (authFailure is not null)
                        return authFailure;

                    TempData["ErrorMessage"] = itemsResult.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
                var clientsResult = await GetClientSelectListAsync(null, cancellationToken);

                if (!clientsResult.Success)
                {
                    authFailure = HandleAuthFailure(clientsResult);
                    if (authFailure is not null)
                        return authFailure;

                    TempData["ErrorMessage"] = clientsResult.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
                var warehousesResult = await GetWarehouseSelectListAsync(null, cancellationToken);

                if (!warehousesResult.Success)
                {
                    authFailure = HandleAuthFailure(warehousesResult);
                    if (authFailure is not null)
                        return authFailure;

                    TempData["ErrorMessage"] = warehousesResult.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
                vm.AvailableItems = itemsResult.Data!;
                vm.AvailableClients = clientsResult.Data!;
                vm.AvailableWarehouses = warehousesResult.Data!;
                return View("Create", vm);
            }
            TempData["SuccessMessage"] = $"Transaction '{result.Data!.TransactionId}' created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(Guid id, CancellationToken cancellationToken)
        {
            var result = await _transactionsService.ApproveTransactionAsync(id, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to approve transaction.";
            }
            else
            {
                TempData["SuccessMessage"] = $"Transaction '{result.Data!}' approved successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
        {
            var result = await _transactionsService.CancelTransactionAsync(id, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to cancel transaction.";
            }
            else
            {
                TempData["SuccessMessage"] = $"Transaction '{result.Data!}' cancelled successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private async Task<ServiceResult<List<ItemSelectOption>>> GetItemSelectListAsync(CancellationToken cancellationToken)
        {
            var itemsResult = await _lookupsService.GetItemsAsync(cancellationToken);

            if (!itemsResult.Success)
                return ServiceResult<List<ItemSelectOption>>.Fail(itemsResult.ErrorMessage ?? "Unable to load items.", itemsResult.ValidationErrors, itemsResult.StatusCode);

            var items = itemsResult.Data!
                .Select(i => new ItemSelectOption
                {
                    Value = i.ItemId.ToString(),
                    Text = i.Name,
                    UnitOfMeasure = i.UnitOfMeasure
                })
                .ToList();

            return ServiceResult<List<ItemSelectOption>>.Ok(items);
        }

        private async Task<ServiceResult<List<SelectListItem>>> GetClientSelectListAsync(Guid? selectedClientId, CancellationToken cancellationToken)
        {
            var clientsResult = await _lookupsService.GetClientsAsync(cancellationToken);

            if (!clientsResult.Success)
                return ServiceResult<List<SelectListItem>>.Fail(clientsResult.ErrorMessage ?? "Unable to load clients.", clientsResult.ValidationErrors, clientsResult.StatusCode);

            var items = clientsResult.Data!
                .Select(c => new SelectListItem
                {
                    Value = c.ClientId.ToString(),
                    Text = c.Name,
                    Selected = selectedClientId.HasValue && c.ClientId == selectedClientId.Value
                })
                .ToList();

            return ServiceResult<List<SelectListItem>>.Ok(items);
        }

        private async Task<ServiceResult<List<SelectListItem>>> GetWarehouseSelectListAsync(Guid? selectedWarehouseId, CancellationToken cancellationToken)
        {
            var warehousesResult = await _lookupsService.GetWarehousesAsync(cancellationToken);

            if (!warehousesResult.Success)
                return ServiceResult<List<SelectListItem>>.Fail(warehousesResult.ErrorMessage ?? "Unable to load warehouses.", warehousesResult.ValidationErrors, warehousesResult.StatusCode);

            var items = warehousesResult.Data!
                .Select(c => new SelectListItem
                {
                    Value = c.WarehouseId.ToString(),
                    Text = c.Name,
                    Selected = selectedWarehouseId.HasValue && c.WarehouseId == selectedWarehouseId.Value
                })
                .ToList();

            return ServiceResult<List<SelectListItem>>.Ok(items);
        }
        #endregion
    }
}
