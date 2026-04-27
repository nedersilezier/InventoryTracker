using InventoryTracker.APIClient;
using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Countries;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Countries;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.Countries;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;

namespace InventoryTracker.WebAdmin.Services
{
    public class CountriesService : ICountriesService
    {
        private readonly ApiHttpClient _apiClient;
        public CountriesService(ApiHttpClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ServiceResult<CountriesIndexViewModel>> GetAllAsync(GetCountriesRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var pageSize = request.PageSize ?? 5;
            var query = new List<string> { $"pageNumber={request.PageNumber}", $"pageSize={pageSize}" };
            if (!string.IsNullOrWhiteSpace(request?.SearchTerm))
            {
                query.Add($"searchTerm={Uri.EscapeDataString(request.SearchTerm)}");
            }
            var url = $"/api/admin/countries?{string.Join("&", query)}";

            var result = await _apiClient.GetAsync<PagedResponse<CountryResponseDTO>>(url, "Failed to load countries.", cancellationToken);
            if (!result.Success)
                return ServiceResult<CountriesIndexViewModel>.Fail(result.ErrorMessage, statusCode: result.StatusCode);
            var pagedResponse = result.Data!;

            var countries = pagedResponse.Items.Select(country => new CountryListItemViewModel
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
            var viewModel = new CountriesIndexViewModel
            {
                Countries = countries,
                SearchTerm = request?.SearchTerm,
                PageSize = pagedResponse?.PageSize ?? pageSize,
                TableFooter = new TableFooterViewModel
                {
                    DisplayedCount = countries.Count,
                    TotalCount = pagedResponse?.TotalCount ?? 0,
                    EntityName = "countries",
                    Pagination = new PaginationViewModel
                    {
                        CurrentPage = pagedResponse?.PageNumber ?? 1,
                        TotalPages = pagedResponse?.TotalPages ?? 1,
                        PageSize = pagedResponse?.PageSize ?? pageSize,
                        Controller = "Countries",
                        Action = "Index",
                        RouteValues = routeValues
                    }
                }
            };
            return ServiceResult<CountriesIndexViewModel>.Ok(viewModel);
        }

        public async Task<ServiceResult<CreateEditCountryViewModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetAsync<CountryResponseDTO>($"/api/admin/countries/{id}", "Failed to load country.", cancellationToken);
            if (!result.Success)
                return ServiceResult<CreateEditCountryViewModel>.Fail(result.ErrorMessage, result.ValidationErrors, result.StatusCode);

            var country = result.Data!;
            var vm = new CreateEditCountryViewModel
            {
                CountryId = country.CountryId,
                Name = country.Name,
                CountryCode = country.Code
            };

            return ServiceResult<CreateEditCountryViewModel>.Ok(vm);
        }
        public async Task<ServiceResult<CreateCountryResponse>> CreateCountryAsync(CreateCountryRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            return await _apiClient.PostAsync<CreateCountryResponse>("/api/admin/countries", request, "Failed to create country.", cancellationToken);
        }
        public async Task<ServiceResult<CreateCountryResponse>> UpdateCountryAsync(Guid id, UpdateCountryRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            return await _apiClient.PutAsync<CreateCountryResponse>($"/api/admin/countries/{id}", request, "Failed to update country.", cancellationToken);
        }

        public async Task<ServiceResult<object>> DeleteCountryAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _apiClient.DeleteAsync<object>($"/api/admin/countries/{id}", fallbackMessage: "Failed to delete country.", cancellationToken: cancellationToken);
        }
    }
}
