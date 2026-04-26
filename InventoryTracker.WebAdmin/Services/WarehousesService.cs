using InventoryTracker.APIClient;
using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Warehouses;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Warehouses;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.Common;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Warehouses;

namespace InventoryTracker.WebAdmin.Services
{
    public class WarehousesService: IWarehousesService
    {
        private readonly ApiHttpClient _apiClient;
        public WarehousesService(ApiHttpClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<ServiceResult<WarehousesIndexViewModel>> GetAllAsync(GetWarehousesRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var pageSize = request.PageSize ?? 5;
            var query = new List<string> { $"pageNumber={request.PageNumber}", $"pageSize={pageSize}" };
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query.Add($"searchTerm={Uri.EscapeDataString(request.SearchTerm)}");
            }

            var url = $"/api/admin/warehouses?{string.Join("&", query)}";
            var result = await _apiClient.GetAsync<PagedResponse<WarehouseResponseDTO>>(url, "Failed to load warehouses.", cancellationToken);
            if(!result.Success)
                return ServiceResult<WarehousesIndexViewModel>.Fail(result.ErrorMessage, result.ValidationErrors, result.StatusCode);

            var pagedResponse = result.Data!;
            var warehouses = pagedResponse?.Items.Select(warehouse => new WarehouseListItemViewModel
            {
                WarehouseId = warehouse.WarehouseId,
                Name = warehouse.Name,
                Code = warehouse.Code,
                AddressLine1 = BuildAddressLine1(warehouse.Address),
                AddressLine2 = BuildAddressLine2(warehouse.Address),
                CountryName = warehouse.Address.CountryName,
                StockEntriesCount = warehouse.StockCount,
                IsActive = warehouse.IsActive
            }).ToList() ?? new List<WarehouseListItemViewModel>();

            var routeValues = new Dictionary<string, string?>
            {
                ["PageSize"] = pageSize.ToString()
            };
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                routeValues["SearchTerm"] = request.SearchTerm;
            }
            var viewModel = new WarehousesIndexViewModel
            {
                Warehouses = warehouses,
                SearchTerm = request.SearchTerm,
                PageSize = pageSize,
                TableFooter = new TableFooterViewModel
                {
                    DisplayedCount = warehouses.Count,
                    TotalCount = pagedResponse?.TotalCount ?? 0,
                    EntityName = "warehouses",
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
            return ServiceResult<WarehousesIndexViewModel>.Ok(viewModel);
        }
        public async Task<ServiceResult<CreateEditWarehouseViewModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetAsync<WarehouseResponseDTO>($"/api/admin/warehouses/{id}", "Failed to load warehouse.", cancellationToken);
            if (!result.Success)
                return ServiceResult<CreateEditWarehouseViewModel>.Fail(result.ErrorMessage, result.ValidationErrors, result.StatusCode);

            var warehouse = result.Data!;
            var vm = new CreateEditWarehouseViewModel
            {
                WarehouseId = warehouse.WarehouseId,
                Name = warehouse.Name,
                WarehouseCode = warehouse.Code,
                Address = new AddressViewModel
                {
                    Street = warehouse.Address.Street,
                    HouseNumber = warehouse.Address.HouseNumber,
                    ApartmentNumber = warehouse.Address.ApartmentNumber,
                    PostalCode = warehouse.Address.PostalCode,
                    City = warehouse.Address.City,
                    CountryId = warehouse.Address.CountryId
                },
            };

            return ServiceResult<CreateEditWarehouseViewModel>.Ok(vm);
        }
        public async Task<ServiceResult<WarehouseDetailsViewModel>> GetDetailsByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetAsync<WarehouseDetailsResponseDTO>($"/api/admin/warehouses/{id}/details", "Failed to load warehouse details.", cancellationToken);
            if (!result.Success)
                return ServiceResult<WarehouseDetailsViewModel>.Fail(result.ErrorMessage, result.ValidationErrors, result.StatusCode);

            var warehouse = result.Data!;
            var vm = new WarehouseDetailsViewModel
            {
                WarehouseId = warehouse.WarehouseId,
                Name = warehouse.Name,
                WarehouseCode = warehouse.Code,
                StockCount = warehouse.StocksCount,
                IsActive = warehouse.IsActive,
                CreatedBy = warehouse.CreatedBy,
                CreatedAt = warehouse.CreatedAt,
                UpdatedBy = warehouse.UpdatedBy,
                UpdatedAt = warehouse.UpdatedAt,
                DeletedBy = warehouse.DeletedBy,
                DeletedAt = warehouse.DeletedAt,
                Address = new AddressDetailsViewModel
                {
                    Street = warehouse.Address.Street,
                    HouseNumber = warehouse.Address.HouseNumber,
                    ApartmentNumber = warehouse.Address.ApartmentNumber,
                    PostalCode = warehouse.Address.PostalCode,
                    City = warehouse.Address.City,
                    CountryName = warehouse.Address.CountryName
                }
            };
            return ServiceResult<WarehouseDetailsViewModel>.Ok(vm);
        }
        public async Task<ServiceResult<CreateWarehouseResponse>> CreateWarehouseAsync(CreateWarehouseRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            return await _apiClient.PostAsync<CreateWarehouseResponse>("/api/admin/warehouses", request, "Failed to create warehouse.", cancellationToken);
        }
        public async Task<ServiceResult<CreateWarehouseResponse>> UpdateWarehouseAsync(Guid id, UpdateWarehouseRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            return await _apiClient.PutAsync<CreateWarehouseResponse>($"/api/admin/warehouses/{id}", request, "Failed to update warehouse.", cancellationToken);
        }
        public async Task<ServiceResult<CreateWarehouseResponse>> DeactivateWarehouseAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _apiClient.PatchAsync<CreateWarehouseResponse>($"/api/admin/warehouses/{id}/deactivate", null, "Failed to deactivate warehouse.", cancellationToken);
        }

        public async Task<ServiceResult<CreateWarehouseResponse>> ActivateWarehouseAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _apiClient.PatchAsync<CreateWarehouseResponse>($"/api/admin/warehouses/{id}/activate", null, "Failed to activate warehouse.", cancellationToken);
        }
        #region Helpers
        private static string BuildAddressLine1(AddressResponseDTO a)
        {
            if (a == null) return string.Empty;

            var street = $"{a.Street} {a.HouseNumber}".Trim();

            if (!string.IsNullOrWhiteSpace(a.ApartmentNumber))
            {
                street += $"/{a.ApartmentNumber}";
            }

            return street;
        }

        private static string BuildAddressLine2(AddressResponseDTO a)
        {
            if (a == null) return string.Empty;
            return $"{a.PostalCode} {a.City}".Trim();
        }
        #endregion
    }
}
