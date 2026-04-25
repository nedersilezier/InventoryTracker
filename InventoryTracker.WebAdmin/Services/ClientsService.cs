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
    public class ClientsService: IClientsService
    {
        private readonly HttpClient _httpClient;
        public ClientsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ServiceResult<ClientsIndexViewModel>> GetAllAsync(GetClientsRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var pageSize = request.PageSize ?? 5;
            var query = new List<string> { $"pageNumber={request.PageNumber}", $"pageSize={pageSize}" };
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query.Add($"searchTerm={Uri.EscapeDataString(request.SearchTerm)}");
            }
            var url = $"/api/admin/clients?{string.Join("&", query)}";
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return await ApiErrorParser.ToFailResult<ClientsIndexViewModel>(response, "Failed to load items.", cancellationToken);

            var pagedResponse = await response.Content.ReadFromJsonAsync<PagedResponse<ClientResponseDTO>>(cancellationToken: cancellationToken);
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
            var url = $"/api/admin/clients/{id}";

            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return await ApiErrorParser.ToFailResult<CreateEditClientViewModel>(response, "Failed to load client.", cancellationToken);

            var client = await response.Content.ReadFromJsonAsync<ClientResponseDTO>(cancellationToken: cancellationToken);

            if (client is null)
                return ServiceResult<CreateEditClientViewModel>.Fail("Failed to parse client details from server.",statusCode: (int)response.StatusCode);

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
            var url = $"/api/admin/clients/{id}/details";

            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return await ApiErrorParser.ToFailResult<ClientDetailsViewModel>(response, "Failed to load client details.", cancellationToken);

            var client = await response.Content.ReadFromJsonAsync<ClientDetailsResponseDTO>(cancellationToken: cancellationToken);

            if (client is null)
                return ServiceResult<ClientDetailsViewModel>.Fail("Failed to parse client details from server.", statusCode: (int)response.StatusCode);

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

            var url = $"/api/admin/clients";
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

            requestMessage.Content = JsonContent.Create(request);

            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return await ApiErrorParser.ToFailResult<CreateClientResponse>(response, "Failed to create client.", cancellationToken);

            var content = await response.Content.ReadFromJsonAsync<CreateClientResponse>(cancellationToken: cancellationToken);

            if (content is null)
                return ServiceResult<CreateClientResponse>.Fail("Failed to parse response from server.", statusCode: (int)response.StatusCode);

            return ServiceResult<CreateClientResponse>.Ok(content);
        }
        public async Task<ServiceResult<CreateClientResponse>> UpdateClientAsync(Guid id, UpdateClientRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var url = $"/api/admin/clients/{id}";
            using var httpRequest = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = JsonContent.Create(request)
            };

            using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return await ApiErrorParser.ToFailResult<CreateClientResponse>(response, "Failed to update client.", cancellationToken);

            var content = await response.Content.ReadFromJsonAsync<CreateClientResponse>(cancellationToken: cancellationToken);

            if (content is null)
                return ServiceResult<CreateClientResponse>.Fail("Failed to parse response from server.", statusCode: (int)response.StatusCode);

            return ServiceResult<CreateClientResponse>.Ok(content);
        }
        public Task<ServiceResult<CreateClientResponse>> DeactivateClientAsync(Guid id, CancellationToken cancellationToken)
        {
            return ChangeClientActiveStateAsync(id, "deactivate", "Failed to deactivate client.", cancellationToken);
        }

        public Task<ServiceResult<CreateClientResponse>> ActivateClientAsync(Guid id, CancellationToken cancellationToken)
        {
            return ChangeClientActiveStateAsync(id, "activate", "Failed to activate client.", cancellationToken);
        }
        #region Helpers
        private async Task<ServiceResult<CreateClientResponse>> ChangeClientActiveStateAsync(Guid id, string action, string fallbackMessage, CancellationToken cancellationToken)
        {
            var url = $"/api/admin/clients/{id}/{action}";
            using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, url)
            {
                Content = JsonContent.Create(new { })
            };

            using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return await ApiErrorParser.ToFailResult<CreateClientResponse>(response, fallbackMessage, cancellationToken);

            var content = await response.Content.ReadFromJsonAsync<CreateClientResponse>(cancellationToken: cancellationToken);

            if (content is null)
                return ServiceResult<CreateClientResponse>.Fail("Failed to parse response from server.", statusCode: (int)response.StatusCode);

            return ServiceResult<CreateClientResponse>.Ok(content);
        }
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
