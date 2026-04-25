using InventoryTracker.APIClient;
using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Responses.Countries;
using InventoryTracker.WebAdmin.Interfaces;

namespace InventoryTracker.WebAdmin.Services
{
    public class LookupsService: ILookupsService
    {
        private readonly HttpClient _httpClient;
        public LookupsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ServiceResult<List<CountryResponseSelectDTO>>> GetCountriesAsync(CancellationToken cancellationToken)
        {
            var url = $"/api/lookups/countries";
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return await ApiErrorParser.ToFailResult<List<CountryResponseSelectDTO>>(response, "Failed to load countries.", cancellationToken);

            var countries = await response.Content.ReadFromJsonAsync<List<CountryResponseSelectDTO>>(cancellationToken: cancellationToken)
                ?? new List<CountryResponseSelectDTO>();
            
            return ServiceResult<List<CountryResponseSelectDTO>>.Ok(countries);
        }
    }
}
