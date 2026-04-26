using InventoryTracker.APIClient;
using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Clients;
using InventoryTracker.Contracts.Responses.Clients;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.Clients;
using InventoryTracker.WebAdmin.ViewModels.Common;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;

namespace InventoryTracker.WebAdmin.Services
{
    public class ClientsService : IClientsService
    {
        private readonly ApiHttpClient _apiClient;
        public ClientsService(HttpClient httpClient, ApiHttpClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<ServiceResult<ClientsIndexViewModel>> GetAllAsync(GetClientsRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var pageSize = request.PageSize ?? 5;
            var query = new List<string> { $"pageNumber={request.PageNumber}", $"pageSize={pageSize}" };
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                query.Add($"searchTerm={Uri.EscapeDataString(request.SearchTerm)}");

            var url = $"/api/admin/clients?{string.Join("&", query)}";

            var result = await _apiClient.GetAsync<PagedResponse<ClientResponseDTO>>(url, "Failed to load clients.", cancellationToken);
            if (!result.Success)
                return ServiceResult<ClientsIndexViewModel>.Fail(result.ErrorMessage, statusCode: result.StatusCode);

            var pagedResponse = result.Data!;
            var clients = pagedResponse?.Items.Select(client => new ClientListItemViewModel
            {
                ClientId = client.ClientId,
                Name = client.Name,
                ClientCode = client.ClientCode,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                AddressLine1 = BuildAddressLine1(client.Address),
                AddressLine2 = BuildAddressLine2(client.Address),
                CountryName = client.Address.CountryName,
                IsActive = client.IsActive,
                Saldo = client.Saldo
            }).ToList() ?? new List<ClientListItemViewModel>();

            var routeValues = new Dictionary<string, string?>
            {
                ["PageSize"] = pageSize.ToString()
            };
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                routeValues["SearchTerm"] = request.SearchTerm;
            }
            var viewModel = new ClientsIndexViewModel
            {
                Clients = clients,
                SearchTerm = request.SearchTerm,
                PageSize = pageSize,
                TableFooter = new TableFooterViewModel
                {
                    DisplayedCount = clients.Count,
                    TotalCount = pagedResponse?.TotalCount ?? 0,
                    EntityName = "clients",
                    Pagination = new PaginationViewModel
                    {
                        CurrentPage = pagedResponse?.PageNumber ?? 1,
                        TotalPages = pagedResponse?.TotalPages ?? 1,
                        PageSize = pagedResponse?.PageSize ?? 1,
                        Controller = "Clients",
                        Action = "Index",
                        RouteValues = routeValues
                    }
                }
            };
            return ServiceResult<ClientsIndexViewModel>.Ok(viewModel);
        }
        public async Task<ServiceResult<CreateEditClientViewModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetAsync<ClientResponseDTO>($"/api/admin/clients/{id}", "Failed to load client.", cancellationToken);
            if (!result.Success)
                return ServiceResult<CreateEditClientViewModel>.Fail(result.ErrorMessage, result.ValidationErrors, result.StatusCode);

            var client = result.Data!;
            var vm = new CreateEditClientViewModel
            {
                ClientId = client.ClientId,
                Name = client.Name,
                ClientCode = client.ClientCode,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                Address = new AddressViewModel
                {
                    Street = client.Address.Street,
                    HouseNumber = client.Address.HouseNumber,
                    ApartmentNumber = client.Address.ApartmentNumber,
                    PostalCode = client.Address.PostalCode,
                    City = client.Address.City,
                    CountryId = client.Address.CountryId
                },
            };

            return ServiceResult<CreateEditClientViewModel>.Ok(vm);
        }
        public async Task<ServiceResult<ClientDetailsViewModel>> GetDetailsByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetAsync<ClientDetailsResponseDTO>($"/api/admin/clients/{id}/details", "Failed to load client details.", cancellationToken);
            if (!result.Success)
                return ServiceResult<ClientDetailsViewModel>.Fail(result.ErrorMessage, result.ValidationErrors, result.StatusCode);

            var client = result.Data!;
            var vm = new ClientDetailsViewModel
            {
                ClientId = client.ClientId,
                Name = client.Name,
                ClientCode = client.ClientCode,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                IsActive = client.IsActive,
                Saldo = client.Saldo,
                CreatedBy = client.CreatedBy,
                CreatedAt = client.CreatedAt,
                UpdatedBy = client.UpdatedBy,
                UpdatedAt = client.UpdatedAt,
                DeletedBy = client.DeletedBy,
                DeletedAt = client.DeletedAt,
                Address = new AddressDetailsViewModel
                {
                    Street = client.Address.Street,
                    HouseNumber = client.Address.HouseNumber,
                    ApartmentNumber = client.Address.ApartmentNumber,
                    PostalCode = client.Address.PostalCode,
                    City = client.Address.City,
                    CountryName = client.Address.CountryName
                }
            };
            return ServiceResult<ClientDetailsViewModel>.Ok(vm);
        }
        public async Task<ServiceResult<CreateClientResponse>> CreateClientAsync(CreateClientRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            return await _apiClient.PostAsync<CreateClientResponse>("/api/admin/clients", request, "Failed to create client.", cancellationToken);
        }
        public async Task<ServiceResult<CreateClientResponse>> UpdateClientAsync(Guid id, UpdateClientRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            return await _apiClient.PutAsync<CreateClientResponse>($"/api/admin/clients/{id}", request, "Failed to update client.", cancellationToken);
        }
        public async Task<ServiceResult<CreateClientResponse>> DeactivateClientAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _apiClient.PatchAsync<CreateClientResponse>($"/api/admin/clients/{id}/deactivate", null, "Failed to deactivate client.", cancellationToken);
        }

        public async Task<ServiceResult<CreateClientResponse>> ActivateClientAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _apiClient.PatchAsync<CreateClientResponse>($"/api/admin/clients/{id}/activate", null, "Failed to activate client.", cancellationToken);
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
