using InventoryTracker.APIClient;
using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Users;
using InventoryTracker.Contracts.Requests.Warehouses;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Users;
using InventoryTracker.Contracts.Responses.Warehouses;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Users;

namespace InventoryTracker.WebAdmin.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApiHttpClient _apiClient;
        public UsersService(ApiHttpClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<ServiceResult<UsersIndexViewModel>> GetAllAsync(GetUsersRequest request, CancellationToken cancellationToken)
        {
            //default page size
            var pageSize = request.PageSize ?? 5;
            var query = new List<string>{ $"pageNumber={request.PageNumber}", $"pageSize={ pageSize }" };

            if (!string.IsNullOrWhiteSpace(request?.SearchTerm))
            {
                query.Add($"searchTerm={Uri.EscapeDataString(request.SearchTerm)}");
            }
            var url = $"/api/admin/users?{string.Join("&", query)}";
            var result = await _apiClient.GetAsync<PagedResponse<UserResponseDTO>>(url, "Failed to load users from the server.", cancellationToken);
            if (!result.Success)
                return ServiceResult<UsersIndexViewModel>.Fail(result.ErrorMessage, result.ValidationErrors, result.StatusCode);

            var pagedResponse = result.Data!;
            var users = pagedResponse?.Items.Select(u => new UserListItemViewModel
            {
                UserId = u.UserId,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                IsActive = u.IsActive,
                UserName = u.UserName,
                Role = u.Role
            }).ToList() ?? new List<UserListItemViewModel>();
            var routeValues = new Dictionary<string, string?>
            {
                ["PageSize"] = pageSize.ToString()
            };
            if (!string.IsNullOrWhiteSpace(request?.SearchTerm))
            {
                routeValues["SearchTerm"] = request.SearchTerm;
            }
            var viewModel = new UsersIndexViewModel
            {
                Users = users,
                SearchTerm = request?.SearchTerm,
                TotalCount = pagedResponse?.TotalCount ?? 0,
                PageSize = pagedResponse?.PageSize ?? pageSize,
                TableFooter = new TableFooterViewModel
                {
                    DisplayedCount = users.Count,
                    TotalCount = pagedResponse?.TotalCount ?? 0,
                    EntityName = "users",
                    Pagination = new PaginationViewModel
                    {
                        CurrentPage = pagedResponse?.PageNumber ?? 1,
                        TotalPages = pagedResponse?.TotalPages ?? 1,
                        PageSize = pagedResponse?.PageSize ?? 1,
                        Controller = "Users",
                        RouteValues = routeValues
                    }
                },
            };
            return ServiceResult<UsersIndexViewModel>.Ok(viewModel);
        }
        public async Task<ServiceResult<UserCreatedResponse>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            return await _apiClient.PostAsync<UserCreatedResponse>("/api/admin/users", request, "Failed to create user.", cancellationToken);
        }
    }
}
