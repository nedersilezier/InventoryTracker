using InventoryTracker.APIClient;
using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Stocks;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Stocks;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Stocks;

namespace InventoryTracker.WebAdmin.Services
{
    public class StocksService: IStocksService
    {
        private readonly ApiHttpClient _apiClient;
        public StocksService(ApiHttpClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ServiceResult<StocksIndexViewModel>> GetAllAsync(GetStocksRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var pageSize = request.PageSize ?? 5;
            var query = new List<string> { $"pageNumber={request.PageNumber}", $"pageSize={pageSize}" };
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query.Add($"searchTerm={Uri.EscapeDataString(request.SearchTerm)}");
            }
            if (request.SelectedWarehouseId.HasValue)
            {
                query.Add($"selectedWarehouseId={request.SelectedWarehouseId}");
            }
            var url = $"/api/admin/stocks?{string.Join("&", query)}";
            

            var result = await _apiClient.GetAsync<PagedResponse<StocksResponseDTO>>(url, "Failed to load stocks.", cancellationToken);
            if (!result.Success)
                return ServiceResult<StocksIndexViewModel>.Fail(result.ErrorMessage, statusCode: result.StatusCode);

            var pagedResponse = result.Data!;
            var stocks = pagedResponse?.Items.Select(stock => new StockListItemViewModel
            {
                StockId = stock.StockId,
                WarehouseId = stock.WarehouseId,
                ItemId = stock.ItemId,
                ItemName = stock.ItemName,
                SKU = stock.SKU,
                UnitOfMeasure = stock.UnitOfMeasure,
                WarehouseName = stock.WarehouseName,
                Quantity = stock.Quantity
            }).ToList() ?? new List<StockListItemViewModel>();

            var routeValues = new Dictionary<string, string?>
            {
                ["PageSize"] = pageSize.ToString()
            };
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                routeValues["SearchTerm"] = request.SearchTerm;
            }
            if (request.SelectedWarehouseId.HasValue)
            {
                routeValues["SelectedWarehouseId"] = request.SelectedWarehouseId.ToString();
            }

            var viewModel = new StocksIndexViewModel
            {
                Stocks = stocks,
                SearchTerm = request.SearchTerm,
                SelectedWarehouseId = request.SelectedWarehouseId,
                PageSize = pageSize,
                TableFooter = new TableFooterViewModel
                {
                    DisplayedCount = stocks.Count,
                    TotalCount = pagedResponse?.TotalCount ?? 0,
                    EntityName = "stocks",
                    Pagination = new PaginationViewModel
                    {
                        CurrentPage = pagedResponse?.PageNumber ?? 1,
                        TotalPages = pagedResponse?.TotalPages ?? 1,
                        PageSize = pagedResponse?.PageSize ?? 1,
                        Controller = "Stocks",
                        Action = "Index",
                        RouteValues = routeValues
                    }
                }
            };
            return ServiceResult<StocksIndexViewModel>.Ok(viewModel);
        }
    }
}
