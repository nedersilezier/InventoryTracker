using InventoryTracker.Contracts.Requests.Countries;
using InventoryTracker.Contracts.Responses.Countries;
using InventoryTracker.WebAdmin.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Services
{
    public class CountriesService : ICountriesService
    {
        private readonly HttpClient _httpClient;
        public CountriesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CountryDTO>> GetAllAsync(CancellationToken cancellationToken)
        {
            var countries = await _httpClient.GetFromJsonAsync<IEnumerable<CountryDTO>>("api/admin/countries", cancellationToken);
            return countries ?? Enumerable.Empty<CountryDTO>();
        }

        public Task<CountryDTO> GetCountryByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<CountryDTO> CreateCountryAsync(CreateCountryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<CountryDTO> UpdateCountryAsync(Guid id, UpdateCountryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
