using InventoryTracker.Contracts.Requests.Warehouses;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Warehouses;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Warehouses;
using System.Net.Http.Headers;

namespace InventoryTracker.WebAdmin.Services
{
    public class WarehousesService: IWarehousesService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WarehousesService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<WarehousesIndexViewModel> GetAllAsync(GetWarehousesRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];

            if (string.IsNullOrEmpty(accessToken))
                throw new UnauthorizedAccessException("Access token is missing.");

            var pageSize = request.PageSize ?? 5;
            var query = new List<string> { $"pageNumber={request.PageNumber}", $"pageSize={pageSize}" };
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query.Add($"searchTerm={Uri.EscapeDataString(request.SearchTerm)}");
            }
            var url = $"/api/admin/warehouses?{string.Join("&", query)}";
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            response.EnsureSuccessStatusCode();

            var pagedResponse = await response.Content.ReadFromJsonAsync<PagedResponse<WarehouseResponseDTO>>(cancellationToken: cancellationToken);
            var warehouses = pagedResponse?.Items.Select(warehouse => new WarehouseListItemViewModel
            {
                WarehouseId = warehouse.WarehouseId,
                Name = warehouse.Name,
                Code = warehouse.Code,
                FullAddress = BuildFullAddress(warehouse.Address),
                CountryName = warehouse.Address.CountryName,
                StockEntriesCount = warehouse.StockCount
            }).ToList() ?? new List<WarehouseListItemViewModel>();

            var routeValues = new Dictionary<string, string?>
            {
                ["PageSize"] = pageSize.ToString()
            };
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                routeValues["SearchTerm"] = request.SearchTerm;
            }
            return new WarehousesIndexViewModel
            {
                Warehouses = warehouses,
                SearchTerm = request.SearchTerm,
                PageSize = pageSize,
                TableFooter = new TableFooterViewModel
                {
                    DisplayedCount = warehouses.Count,
                    TotalCount = pagedResponse?.TotalCount ?? 0,
                    EntityName = "countries",
                    Pagination = new PaginationViewModel
                    {
                        CurrentPage = pagedResponse?.PageNumber ?? 1,
                        TotalPages = pagedResponse?.TotalPages ?? 1,
                        PageSize = pagedResponse?.PageSize ?? 1,
                        Controller = "Warehouses",
                        Action = "Index",
                        RouteValues = routeValues
                    }
                }
            };
        }
        #region Helpers
        private static string BuildFullAddress(AddressResponseDTO a)
        {
            if (a == null) return string.Empty;

            var street = $"{a.Street} {a.HouseNumber}".Trim();

            if (!string.IsNullOrWhiteSpace(a.ApartmentNumber))
            {
                street += $"/{a.ApartmentNumber}";
            }

            var cityPart = $"{a.PostalCode} {a.City}".Trim();

            var parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(street))
                parts.Add(street);

            if (!string.IsNullOrWhiteSpace(cityPart))
                parts.Add(cityPart);

            return string.Join(", ", parts);
        }
        #endregion
    }
}
