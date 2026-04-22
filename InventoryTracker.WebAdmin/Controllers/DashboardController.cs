using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ITransactionsService _transactionsService;
        public DashboardController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var transactions = await _transactionsService.GetRecentTransactionsAsync(5, cancellationToken);
            return View(new DashboardViewModel { RecentTransactions = transactions.ToList() });
        }
    }
}
