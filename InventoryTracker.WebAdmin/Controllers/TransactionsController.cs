using InventoryTracker.Contracts.Requests.Transactions;
using InventoryTracker.Shared.Enums;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Transactions;
using InventoryTracker.WebAdmin.ViewModels.Warehouses;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransactionsService _transactionsService;
        public TransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }
        public async Task<IActionResult> Index(GetTransactionsRequest request, CancellationToken cancellationToken)
        {
            if (!request.DateFrom.HasValue && !request.DateTo.HasValue)
            {
                var today = DateTime.Today;
                request.DateFrom = today;
                request.DateTo = today;
            }
            var transactionsListViewModel = await _transactionsService.GetAllTransactionsAsync(request, cancellationToken);
            return View(transactionsListViewModel);
        }
    }
}
