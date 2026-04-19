using InventoryTracker.Contracts.Requests.Countries;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Countries;
using InventoryTracker.Contracts.Responses.Users;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.Countries;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
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

        public async Task<CountriesIndexViewModel> GetAllAsync(GetCountriesRequest request, CancellationToken cancellationToken)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];

            if (string.IsNullOrEmpty(accessToken))
                throw new UnauthorizedAccessException("Access token is missing.");

            var pageSize = request.PageSize ?? 1;
            var query = new List<string> { $"pageNumber={request.PageNumber}", $"pageSize={pageSize}" };
            if (!string.IsNullOrWhiteSpace(request?.SearchTerm))
            {
                query.Add($"searchTerm={Uri.EscapeDataString(request.SearchTerm)}");
            }
            var url = $"/api/admin/countries?{string.Join("&", query)}";
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            response.EnsureSuccessStatusCode();

            var pagedResponse = await response.Content.ReadFromJsonAsync<PagedResponse<CountryResponseDTO>>(cancellationToken: cancellationToken);
            var countries = pagedResponse?.Items.Select(country => new CountryListItemViewModel
            {
                CountryId = country.CountryId,
                Name = country.Name,
                Code = country.Code,
                CreatedBy = country.CreatedBy ?? string.Empty,
                CreatedAt = country.CreatedAt,
                UpdatedBy = country.UpdatedBy,
                UpdatedAt = country.UpdatedAt
            }).ToList() ?? new List<CountryListItemViewModel>();
            var routeValues = new Dictionary<string, string?>
            {
                ["PageSize"] = pageSize.ToString()
            };
            if (!string.IsNullOrWhiteSpace(request?.SearchTerm))
            {
                routeValues["SearchTerm"] = request.SearchTerm;
            }
            return new CountriesIndexViewModel
            {
                Countries = countries,
                SearchTerm = request?.SearchTerm,
                TableFooter = new TableFooterViewModel
                {
                    DisplayedCount = countries.Count,
                    TotalCount = pagedResponse?.TotalCount ?? 0,
                    EntityName = "countries",
                    Pagination = new PaginationViewModel
                    {
                        CurrentPage = pagedResponse?.PageNumber ?? 1,
                        TotalPages = pagedResponse?.TotalPages ?? 1,
                        PageSize = pagedResponse?.PageSize ?? 1,
                        Controller = "Countries",
                        Action = "Index",
                        RouteValues = routeValues
                    }
                }
            };
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
