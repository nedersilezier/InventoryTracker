using InventoryTracker.Contracts.Requests.Stocks;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Stocks;
using InventoryTracker.WebAdmin.ViewModels.Warehouses;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class StocksController : Controller
    {
        private readonly IStocksService _stocksService;
        public StocksController(IStocksService stocksService)
        {
            _stocksService = stocksService;
        }
        public async Task<IActionResult> Index(GetStocksRequest request, CancellationToken cancellationToken)
        {
            var stocksListViewModel = await _stocksService.GetAllAsync(request, cancellationToken);
            return View(stocksListViewModel);
        }
    }
}
