using InventoryTracker.Contracts.Requests.Items;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Items;
using InventoryTracker.WebAdmin.Helpers;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Items;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
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
        public async Task<ItemsIndexViewModel> GetAllAsync(GetItemsRequest request, CancellationToken cancellationToken)
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
            var url = $"/api/admin/items?{string.Join("&", query)}";
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            response.EnsureSuccessStatusCode();

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
            return new ItemsIndexViewModel
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
        }
        public async Task<ServiceResult<CreateEditItemViewModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];
            if (string.IsNullOrEmpty(accessToken))
                return ServiceResult<CreateEditItemViewModel>.Fail("Access token is missing.");

            var url = $"/api/admin/items/{id}";

            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var item = await response.Content.ReadFromJsonAsync<ItemResponseDTO>(cancellationToken: cancellationToken);

                if (item is null)
                    return ServiceResult<CreateEditItemViewModel>.Fail("Failed to parse item details from server.");

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

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            ProblemDetails? problem = null;

            if (!string.IsNullOrWhiteSpace(responseBody))
            {
                try
                {
                    problem = JsonSerializer.Deserialize<ProblemDetails>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                catch (JsonException)
                {
                }
            }

            return ServiceResult<CreateEditItemViewModel>.Fail(
                problem?.Detail ?? $"Failed to load item. Status code: {(int)response.StatusCode}");
        }
        public async Task<ServiceResult<ItemDetailsViewModel>> GetDetailsByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];
            if (string.IsNullOrEmpty(accessToken))
                return ServiceResult<ItemDetailsViewModel>.Fail("Access token is missing.");

            var url = $"/api/admin/items/{id}/details";

            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var item = await response.Content.ReadFromJsonAsync<ItemDetailsResponseDTO>(cancellationToken: cancellationToken);

                if (item is null)
                    return ServiceResult<ItemDetailsViewModel>.Fail("Failed to parse item details from server.");

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

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            ProblemDetails? problem = null;

            if (!string.IsNullOrWhiteSpace(responseBody))
            {
                try
                {
                    problem = JsonSerializer.Deserialize<ProblemDetails>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                catch (JsonException)
                {
                }
            }

            return ServiceResult<ItemDetailsViewModel>.Fail(
                problem?.Detail ?? $"Failed to load item. Status code: {(int)response.StatusCode}");
        }

        public async Task<ServiceResult<CreateItemResponse>> CreateItemAsync(CreateItemRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];

            if (string.IsNullOrEmpty(accessToken))
                throw new UnauthorizedAccessException("Access token is missing.");
            var url = $"/api/admin/items";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.PostAsJsonAsync(url, request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<CreateItemResponse>(cancellationToken);
                if (content == null)
                    return ServiceResult<CreateItemResponse>.Fail("Failed to parse response from server.");

                return ServiceResult<CreateItemResponse>.Ok(content);
            }
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: cancellationToken);
            Dictionary<string, string[]>? validationErrors = null;
            if (problem?.Extensions != null && problem.Extensions.TryGetValue("errors", out var errors))
            {
                var json = JsonSerializer.Serialize(errors);
                validationErrors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(json);
            }
            return ServiceResult<CreateItemResponse>.Fail(problem?.Detail ?? "Request failer", validationErrors);
        }
        public async Task<ServiceResult<CreateItemResponse>> UpdateItemAsync(Guid id, UpdateItemRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];
            if (string.IsNullOrWhiteSpace(accessToken))
                return ServiceResult<CreateItemResponse>.Fail("Access token is missing.");

            var url = $"/api/admin/items/{id}";

            using var httpRequest = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = JsonContent.Create(request)
            };

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<CreateItemResponse>(cancellationToken: cancellationToken);

                if (content is null)
                    return ServiceResult<CreateItemResponse>.Fail("Failed to parse response from server.");

                return ServiceResult<CreateItemResponse>.Ok(content);
            }

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            ProblemDetails? problem = null;
            Dictionary<string, string[]>? validationErrors = null;

            if (!string.IsNullOrWhiteSpace(responseBody))
            {
                try
                {
                    problem = JsonSerializer.Deserialize<ProblemDetails>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (problem?.Extensions != null && problem.Extensions.TryGetValue("errors", out var errors))
                    {
                        var json = JsonSerializer.Serialize(errors);
                        validationErrors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(json);
                    }
                }
                catch (JsonException)
                {
                }
            }

            return ServiceResult<CreateItemResponse>.Fail(
                problem?.Detail ?? $"Request failed. Status code: {(int)response.StatusCode}", validationErrors);
        }

        public async Task<ServiceResult<CreateItemResponse>> DeactivateItemAsync(Guid id, CancellationToken cancellationToken)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];

            if (string.IsNullOrWhiteSpace(accessToken))
                return ServiceResult<CreateItemResponse>.Fail("Access token is missing.");

            var url = $"/api/admin/items/{id}/deactivate";

            using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, url)
            {
                Content = JsonContent.Create(new { })
            };

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<CreateItemResponse>(cancellationToken: cancellationToken);

                if (content is null)
                    return ServiceResult<CreateItemResponse>.Fail("Failed to parse response from server.");

                return ServiceResult<CreateItemResponse>.Ok(content);
            }

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            ProblemDetails? problem = null;
            Dictionary<string, string[]>? validationErrors = null;

            if (!string.IsNullOrWhiteSpace(responseBody))
            {
                try
                {
                    problem = JsonSerializer.Deserialize<ProblemDetails>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (problem?.Extensions != null &&
                        problem.Extensions.TryGetValue("errors", out var errors))
                    {
                        var json = JsonSerializer.Serialize(errors);
                        validationErrors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(json);
                    }
                }
                catch (JsonException)
                {
                }
            }

            return ServiceResult<CreateItemResponse>.Fail(
                problem?.Detail ?? $"Request failed. Status code: {(int)response.StatusCode}",
                validationErrors);
        }
        public async Task<ServiceResult<CreateItemResponse>> ActivateItemAsync(Guid id, CancellationToken cancellationToken)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];

            if (string.IsNullOrWhiteSpace(accessToken))
                return ServiceResult<CreateItemResponse>.Fail("Access token is missing.");

            var url = $"/api/admin/items/{id}/activate";

            using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, url)
            {
                Content = JsonContent.Create(new { })
            };

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<CreateItemResponse>(cancellationToken: cancellationToken);

                if (content is null)
                    return ServiceResult<CreateItemResponse>.Fail("Failed to parse response from server.");

                return ServiceResult<CreateItemResponse>.Ok(content);
            }

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            ProblemDetails? problem = null;
            Dictionary<string, string[]>? validationErrors = null;

            if (!string.IsNullOrWhiteSpace(responseBody))
            {
                try
                {
                    problem = JsonSerializer.Deserialize<ProblemDetails>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (problem?.Extensions != null &&
                        problem.Extensions.TryGetValue("errors", out var errors))
                    {
                        var json = JsonSerializer.Serialize(errors);
                        validationErrors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(json);
                    }
                }
                catch (JsonException)
                {
                }
            }

            return ServiceResult<CreateItemResponse>.Fail(
                problem?.Detail ?? $"Request failed. Status code: {(int)response.StatusCode}",
                validationErrors);
        }
    }
}
