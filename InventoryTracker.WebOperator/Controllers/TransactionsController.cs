using InventoryTracker.Contracts.Requests.Transactions;
using InventoryTracker.WebOperator.Interfaces;
using InventoryTracker.WebOperator.ViewModels;
using InventoryTracker.WebOperator.ViewModels.Transactions;
using Microsoft.AspNetCore.Mvc;


namespace InventoryTracker.WebOperator.Controllers
{
    public class TransactionsController : BaseController
    {
        private readonly ITransactionsService _transactionsService;
        public TransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
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

                return View(new OperatorTransactionsIndexViewModel
                {
                    TotalCount = 0,
                    DisplayedCount = 0,
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
                    Pagination = new PaginationViewModel
                    {
                        CurrentPage = 1,
                        TotalPages = 1,
                        PageSize = request.PageSize,
                        Controller = "Transactions",
                        Action = "Index"
                    }
                });
            }
            return View(result.Data);
        }
    }
}
