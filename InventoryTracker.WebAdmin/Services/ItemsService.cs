using InventoryTracker.APIClient;
using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Items;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Items;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Items;

namespace InventoryTracker.WebAdmin.Services
{
    public class ItemsService : IItemsService
    {
        private readonly ApiHttpClient _apiClient;

        public ItemsService(ApiHttpClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<ServiceResult<ItemsIndexViewModel>> GetAllAsync(GetItemsRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var pageSize = request.PageSize ?? 5;

            var query = new List<string>{ $"pageNumber={request.PageNumber}", $"pageSize={pageSize}" };

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                query.Add($"searchTerm={Uri.EscapeDataString(request.SearchTerm)}");

            var url = $"/api/admin/items?{string.Join("&", query)}";

            var result = await _apiClient.GetAsync<PagedResponse<ItemResponseDTO>>(url, "Failed to load items.", cancellationToken);

            if (!result.Success)
                return ServiceResult<ItemsIndexViewModel>.Fail(result.ErrorMessage, statusCode: result.StatusCode);

            var pagedResponse = result.Data!;

            var items = pagedResponse.Items.Select(item => new ItemListItemViewModel
            {
                ItemId = item.ItemId,
                Name = item.Name,
                SKU = item.SKU,
                UnitOfMeasure = item.UnitOfMeasure,
                CreditValue = item.CreditValue,
                Weight = item.Weight,
                Description = item.Description,
                IsActive = item.IsActive
            }).ToList();

            var routeValues = new Dictionary<string, string?>
            {
                ["PageSize"] = pageSize.ToString()
            };

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                routeValues["SearchTerm"] = request.SearchTerm;

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
                        PageSize = pagedResponse?.PageSize ?? 5,
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
            var result = await _apiClient.GetAsync<ItemResponseDTO>($"/api/admin/items/{id}", "Failed to load item.", cancellationToken);

            if (!result.Success)
                return ServiceResult<CreateEditItemViewModel>.Fail(result.ErrorMessage, result.ValidationErrors, result.StatusCode);

            var item = result.Data!;

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
            var result = await _apiClient.GetAsync<ItemDetailsResponseDTO>($"/api/admin/items/{id}/details", "Failed to load item details.", cancellationToken);

            if (!result.Success)
                return ServiceResult<ItemDetailsViewModel>.Fail(result.ErrorMessage, result.ValidationErrors, result.StatusCode);

            var item = result.Data!;

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

            return await _apiClient.PostAsync<CreateItemResponse>("/api/admin/items", request, "Failed to create item.", cancellationToken);
        }
        public async Task<ServiceResult<CreateItemResponse>> UpdateItemAsync(Guid id, UpdateItemRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            return await _apiClient.PutAsync<CreateItemResponse>($"/api/admin/items/{id}", request, "Failed to update item.",    cancellationToken);
        }
        public async Task<ServiceResult<CreateItemResponse>> DeactivateItemAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _apiClient.PatchAsync<CreateItemResponse>($"/api/admin/items/{id}/deactivate", null, "Failed to deactivate item.", cancellationToken);
        }
        public async Task<ServiceResult<CreateItemResponse>> ActivateItemAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _apiClient.PatchAsync<CreateItemResponse>($"/api/admin/items/{id}/activate", null, "Failed to activate item.", cancellationToken);
        }
    }
}