using InventoryTracker.APIClient;
using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Items;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Items;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Items;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace InventoryTracker.WebAdmin.Services
{
    public class ItemsService : IItemsService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ItemsService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ServiceResult<ItemsIndexViewModel>> GetAllAsync(GetItemsRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];

            if (string.IsNullOrEmpty(accessToken))
                return ServiceResult<ItemsIndexViewModel>.Fail("Access token is missing.", statusCode: 401);

            var pageSize = request.PageSize ?? 5;
            var query = new List<string> { $"pageNumber={request.PageNumber}", $"pageSize={pageSize}" };
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query.Add($"searchTerm={Uri.EscapeDataString(request.SearchTerm)}");
            }
            var url = $"/api/admin/items?{string.Join("&", query)}";
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return await ApiErrorParser.ToFailResult<ItemsIndexViewModel>(response, "Failed to load items.", cancellationToken);

            var pagedResponse = await response.Content.ReadFromJsonAsync<PagedResponse<ItemResponseDTO>>(cancellationToken: cancellationToken)
                ?? new PagedResponse<ItemResponseDTO>();

            var items = pagedResponse?.Items.Select(item => new ItemListItemViewModel
            {
                ItemId = item.ItemId,
                Name = item.Name,
                SKU = item.SKU,
                UnitOfMeasure = item.UnitOfMeasure,
                CreditValue = item.CreditValue,
                Weight = item.Weight,
                Description = item.Description,
                IsActive = item.IsActive
            }).ToList() ?? new List<ItemListItemViewModel>();

            var routeValues = new Dictionary<string, string?>
            {
                ["PageSize"] = pageSize.ToString()
            };
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                routeValues["SearchTerm"] = request.SearchTerm;
            }
            var viewModel = new ItemsIndexViewModel
            {
                Items = items,
                SearchTerm = request.SearchTerm,
                PageSize = pageSize,
                TotalActiveItems = pagedResponse?.TotalActive ?? 0,
                TableFooter = new TableFooterViewModel
                {
                    DisplayedCount = items.Count,
                    TotalCount = pagedResponse?.TotalCount ?? 0,
                    EntityName = "items",
                    Pagination = new PaginationViewModel
                    {
                        CurrentPage = pagedResponse?.PageNumber ?? 1,
                        TotalPages = pagedResponse?.TotalPages ?? 1,
                        PageSize = pagedResponse?.PageSize ?? 1,
                        Controller = "Items",
                        Action = "Index",
                        RouteValues = routeValues
                    }
                }
            };
            return ServiceResult<ItemsIndexViewModel>.Ok(viewModel);
        }
        public async Task<ServiceResult<CreateEditItemViewModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];
            if (string.IsNullOrWhiteSpace(accessToken))
                return ServiceResult<CreateEditItemViewModel>.Fail("Access token is missing.", statusCode: StatusCodes.Status401Unauthorized);

            var url = $"/api/admin/items/{id}";

            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return await ApiErrorParser.ToFailResult<CreateEditItemViewModel>(
                    response,
                    "Failed to load item.",
                    cancellationToken);
            }

            var item = await response.Content.ReadFromJsonAsync<ItemResponseDTO>(
                cancellationToken: cancellationToken);

            if (item is null)
            {
                return ServiceResult<CreateEditItemViewModel>.Fail(
                    "Failed to parse item details from server.",
                    statusCode: (int)response.StatusCode);
            }

            var vm = new CreateEditItemViewModel
            {
                ItemId = item.ItemId,
                Name = item.Name,
                SKU = item.SKU,
                UnitOfMeasure = item.UnitOfMeasure,
                CreditValue = item.CreditValue,
                Weight = item.Weight,
                Description = item.Description
            };

            return ServiceResult<CreateEditItemViewModel>.Ok(vm);
        }
        public async Task<ServiceResult<ItemDetailsViewModel>> GetDetailsByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];
            if (string.IsNullOrWhiteSpace(accessToken))
                return ServiceResult<ItemDetailsViewModel>.Fail("Access token is missing.", statusCode: StatusCodes.Status401Unauthorized);

            var url = $"/api/admin/items/{id}/details";

            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return await ApiErrorParser.ToFailResult<ItemDetailsViewModel>(response, "Failed to load item details.", cancellationToken);

            var item = await response.Content.ReadFromJsonAsync<ItemDetailsResponseDTO>(cancellationToken: cancellationToken);

            if (item is null)
                return ServiceResult<ItemDetailsViewModel>.Fail("Failed to parse item details from server.", statusCode: (int)response.StatusCode);

            var vm = new ItemDetailsViewModel
            {
                ItemId = item.ItemId,
                Name = item.Name,
                SKU = item.SKU,
                UnitOfMeasure = item.UnitOfMeasure,
                CreditValue = item.CreditValue,
                Weight = item.Weight,
                Description = item.Description,
                IsActive = item.IsActive,
                CreatedBy = item.CreatedBy,
                CreatedAt = item.CreatedAt,
                UpdatedBy = item.UpdatedBy,
                UpdatedAt = item.UpdatedAt,
                DeletedBy = item.DeletedBy,
                DeletedAt = item.DeletedAt
            };

            return ServiceResult<ItemDetailsViewModel>.Ok(vm);
        }

        public async Task<ServiceResult<CreateItemResponse>> CreateItemAsync(CreateItemRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];
            if (string.IsNullOrWhiteSpace(accessToken))
                return ServiceResult<CreateItemResponse>.Fail("Access token is missing.", statusCode: StatusCodes.Status401Unauthorized);

            var url = $"/api/admin/items";
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            requestMessage.Content = JsonContent.Create(request);

            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return await ApiErrorParser.ToFailResult<CreateItemResponse>(response, "Failed to create item.", cancellationToken);

            var content = await response.Content.ReadFromJsonAsync<CreateItemResponse>(cancellationToken: cancellationToken);

            if (content is null)
                return ServiceResult<CreateItemResponse>.Fail("Failed to parse response from server.", statusCode: (int)response.StatusCode);

            return ServiceResult<CreateItemResponse>.Ok(content);
        }
        public async Task<ServiceResult<CreateItemResponse>> UpdateItemAsync(Guid id, UpdateItemRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];
            if (string.IsNullOrWhiteSpace(accessToken))
                return ServiceResult<CreateItemResponse>.Fail("Access token is missing.", statusCode: StatusCodes.Status401Unauthorized);

            var url = $"/api/admin/items/{id}";

            using var httpRequest = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = JsonContent.Create(request)
            };

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return await ApiErrorParser.ToFailResult<CreateItemResponse>(
                    response,
                    "Failed to update item.",
                    cancellationToken);
            }

            var content = await response.Content.ReadFromJsonAsync<CreateItemResponse>(
                cancellationToken: cancellationToken);

            if (content is null)
            {
                return ServiceResult<CreateItemResponse>.Fail(
                    "Failed to parse response from server.",
                    statusCode: (int)response.StatusCode);
            }

            return ServiceResult<CreateItemResponse>.Ok(content);
        }
        public Task<ServiceResult<CreateItemResponse>> DeactivateItemAsync(Guid id, CancellationToken cancellationToken)
        {
            return ChangeItemActiveStateAsync(id, "deactivate", "Failed to deactivate item.", cancellationToken);
        }

        public Task<ServiceResult<CreateItemResponse>> ActivateItemAsync(Guid id, CancellationToken cancellationToken)
        {
            return ChangeItemActiveStateAsync(id, "activate", "Failed to activate item.", cancellationToken);
        }

        private async Task<ServiceResult<CreateItemResponse>> ChangeItemActiveStateAsync(Guid id, string action, string fallbackMessage, CancellationToken cancellationToken)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];

            if (string.IsNullOrWhiteSpace(accessToken))
                return ServiceResult<CreateItemResponse>.Fail("Access token is missing.", statusCode: StatusCodes.Status401Unauthorized);

            var url = $"/api/admin/items/{id}/{action}";
            using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, url)
            {
                Content = JsonContent.Create(new { })
            };

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return await ApiErrorParser.ToFailResult<CreateItemResponse>(response, fallbackMessage, cancellationToken);

            var content = await response.Content.ReadFromJsonAsync<CreateItemResponse>(cancellationToken: cancellationToken);

            if (content is null)
                return ServiceResult<CreateItemResponse>.Fail("Failed to parse response from server.", statusCode: (int)response.StatusCode);

            return ServiceResult<CreateItemResponse>.Ok(content);
        }
    }
}
