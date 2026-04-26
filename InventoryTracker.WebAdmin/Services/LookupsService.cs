using InventoryTracker.APIClient;
using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Responses.Countries;
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
    }
}
