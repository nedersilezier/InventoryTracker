using InventoryTracker.Contracts.Requests.Stocks;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Stocks;
using InventoryTracker.Contracts.Responses.Warehouses;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Stocks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;

namespace InventoryTracker.WebAdmin.Services
{
    public class StocksService: IStocksService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StocksService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<StocksIndexViewModel> GetAllAsync(GetStocksRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];

            if (string.IsNullOrEmpty(accessToken))
                throw new UnauthorizedAccessException("Access token is missing.");

            var pageSize = request.PageSize ?? 1;
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
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            response.EnsureSuccessStatusCode();

            var pagedResponse = await response.Content.ReadFromJsonAsync<PagedResponse<StocksResponseDTO>>(cancellationToken: cancellationToken);
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

            // get warehouses for select list
            var urlForSelect = $"/api/lookups/warehouses";
            using var requestMessageForSelect = new HttpRequestMessage(HttpMethod.Get, urlForSelect);
            requestMessageForSelect.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var responseForSelect = await _httpClient.SendAsync(requestMessageForSelect, cancellationToken);
            responseForSelect.EnsureSuccessStatusCode();
            var warehouses = await responseForSelect.Content.ReadFromJsonAsync<List<WarehouseResponseSelectDTO>>(cancellationToken: cancellationToken)
                ?? new List<WarehouseResponseSelectDTO>();

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
            var warehousesSelectList = new List<SelectListItem>();
            warehousesSelectList.Add(new SelectListItem
            {
                Text = "All Warehouses",
                Value = ""
            });
            warehousesSelectList.AddRange(warehouses.Select(w => new SelectListItem
            {
                Text = w.Name,
                Value = w.WarehouseId.ToString(),
            }).ToList());

            return new StocksIndexViewModel
            {
                Stocks = stocks,
                Warehouses = warehousesSelectList,
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
        }
    }
}
