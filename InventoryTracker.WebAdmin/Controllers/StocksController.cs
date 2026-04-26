using InventoryTracker.Contracts.Requests.Stocks;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Stocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class StocksController : BaseController
    {
        private readonly IStocksService _stocksService;
        private readonly ILookupsService _lookupsService;
        public StocksController(IStocksService stocksService, ILookupsService lookupsService)
        {
            _stocksService = stocksService;
            _lookupsService = lookupsService;
        }

        public async Task<IActionResult> Index(GetStocksRequest request, CancellationToken cancellationToken)
        {
            var warehousesResult = await _lookupsService.GetWarehousesAsync(cancellationToken);

            if (!warehousesResult.Success)
            {
                var authFailure = HandleAuthFailure(warehousesResult);
                if (authFailure is not null)
                    return authFailure;
                TempData["ErrorMessage"] = warehousesResult.ErrorMessage ?? "Unable to load warehouses.";
                return View(new StocksIndexViewModel
                {
                    Stocks = new List<StockListItemViewModel>(),
                    SearchTerm = request.SearchTerm,
                    SelectedWarehouseId = null,
                    PageSize = request.PageSize ?? 5,
                    TableFooter = new TableFooterViewModel
                    {
                        DisplayedCount = 0,
                        TotalCount = 0,
                        EntityName = "stocks",
                        Pagination = new PaginationViewModel
                        {
                            CurrentPage = request.PageNumber,
                            TotalPages = 1,
                            PageSize = request.PageSize ?? 5,
                            Controller = "Stocks",
                            Action = "Index"
                        }
                    }
                });
            }
            var items = warehousesResult.Data!
                .Select(w => new SelectListItem
                {
                    Value = w.WarehouseId.ToString(),
                    Text = w.Name,
                })
                .ToList();

            var stocksResult = await _stocksService.GetAllAsync(request, cancellationToken);
            if(!stocksResult.Success)
            {
                var authFailure = HandleAuthFailure(stocksResult);
                if (authFailure is not null)
                    return authFailure;
                TempData["ErrorMessage"] = stocksResult.ErrorMessage ?? "Unable to load stocks.";
                return View(new StocksIndexViewModel
                {
                    Stocks = new List<StockListItemViewModel>(),
                    SearchTerm = request.SearchTerm,
                    SelectedWarehouseId = null,
                    PageSize = request.PageSize ?? 5,
                    TableFooter = new TableFooterViewModel
                    {
                        DisplayedCount = 0,
                        TotalCount = 0,
                        EntityName = "stocks",
                        Pagination = new PaginationViewModel
                        {
                            CurrentPage = request.PageNumber,
                            TotalPages = 1,
                            PageSize = request.PageSize ?? 5,
                            Controller = "Stocks",
                            Action = "Index"
                        }
                    }
                });
            }
            stocksResult.Data!.Warehouses = items;
            return View(stocksResult.Data);
        }
    }
}
