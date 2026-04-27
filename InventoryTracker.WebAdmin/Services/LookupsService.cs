using InventoryTracker.APIClient;
using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Responses.Clients;
using InventoryTracker.Contracts.Responses.Countries;
using InventoryTracker.Contracts.Responses.Items;
using InventoryTracker.Contracts.Responses.Users;
using InventoryTracker.Contracts.Responses.Warehouses;
using InventoryTracker.WebAdmin.Interfaces;

namespace InventoryTracker.WebAdmin.Services
{
    public class LookupsService: ILookupsService
    {
        private ApiHttpClient _apiClient;
        public LookupsService(HttpClient httpClient, ApiHttpClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<ServiceResult<List<CountryResponseSelectDTO>>> GetCountriesAsync(CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetAsync<List<CountryResponseSelectDTO>>("/api/lookups/countries", "Failed to load countries.", cancellationToken);
            if(!result.Success)
                return ServiceResult<List<CountryResponseSelectDTO>>.Fail(result.ErrorMessage, statusCode: result.StatusCode);

            return ServiceResult<List<CountryResponseSelectDTO>>.Ok(result.Data!);
        }

        public async Task<ServiceResult<List<WarehouseResponseSelectDTO>>> GetWarehousesAsync(CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetAsync<List<WarehouseResponseSelectDTO>>("/api/lookups/warehouses", "Failed to load warehouses.", cancellationToken);
            if(!result.Success)
                return ServiceResult<List<WarehouseResponseSelectDTO>>.Fail(result.ErrorMessage, statusCode: result.StatusCode);

            return ServiceResult<List<WarehouseResponseSelectDTO>>.Ok(result.Data!);
        }
        public async Task<ServiceResult<List<ClientResponseSelectDTO>>> GetClientsAsync(CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetAsync<List<ClientResponseSelectDTO>>("/api/lookups/clients", "Failed to load clients.", cancellationToken);
            if(!result.Success)
                return ServiceResult<List<ClientResponseSelectDTO>>.Fail(result.ErrorMessage, statusCode: result.StatusCode);

            return ServiceResult<List<ClientResponseSelectDTO>>.Ok(result.Data!);
        }
        public async Task<ServiceResult<List<ItemResponseSelectDTO>>> GetItemsAsync(CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetAsync<List<ItemResponseSelectDTO>>("/api/lookups/items", "Failed to load items.", cancellationToken);
            if(!result.Success)
                return ServiceResult<List<ItemResponseSelectDTO>>.Fail(result.ErrorMessage, statusCode: result.StatusCode);

            return ServiceResult<List<ItemResponseSelectDTO>>.Ok(result.Data!);
        }

        public async Task<ServiceResult<List<RoleResponseDTO>>> GetRolesAsync(CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetAsync<List<RoleResponseDTO>>("/api/admin/users/roles", "Failed to load roles.", cancellationToken);
            if(!result.Success)
                return ServiceResult<List<RoleResponseDTO>>.Fail(result.ErrorMessage, statusCode: result.StatusCode);

            return ServiceResult<List<RoleResponseDTO>>.Ok(result.Data!);
        }
    }
}
