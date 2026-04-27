using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly ITransactionsService _transactionsService;
        public DashboardController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var result = await _transactionsService.GetRecentTransactionsAsync(5, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to load recent transactions.";

                return View(new DashboardViewModel
                {
                    TotalItems = 0,
                    ActiveClients = 0,
                    Warehouses = 0,
                    StockCapacity = 0,
                });
            }
            var vm = new DashboardViewModel
            {
                TotalItems = 0,
                ActiveClients = 0,
                Warehouses = 0,
                StockCapacity = 0,
                RecentTransactions = result.Data!.ToList()
            };
            return View(vm);
        }
    }
}
