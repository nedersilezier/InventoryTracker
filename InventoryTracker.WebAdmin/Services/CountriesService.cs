using InventoryTracker.Contracts.Requests.Countries;
using InventoryTracker.Contracts.Responses.Countries;
using InventoryTracker.WebAdmin.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace InventoryTracker.WebAdmin.Services
{
    public class CountriesService : ICountriesService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CountriesService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<CountryDTO>> GetAllAsync(CancellationToken cancellationToken)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];

            if (string.IsNullOrEmpty(accessToken))
                throw new UnauthorizedAccessException("Access token is missing.");

            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/admin/countries");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<CountryDTO>>(cancellationToken: cancellationToken)
                   ?? new List<CountryDTO>();
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
