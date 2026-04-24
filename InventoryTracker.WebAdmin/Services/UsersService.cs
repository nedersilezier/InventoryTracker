using InventoryTracker.Contracts.Requests.Users;
using InventoryTracker.Contracts.Responses.Users;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Users;
using System.Net.Http.Headers;

namespace InventoryTracker.WebAdmin.Services
{
    public class UsersService : IUsersService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsersService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<UsersIndexViewModel> GetAllAsync(GetUsersRequest request, CancellationToken cancellationToken)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];

            if (string.IsNullOrEmpty(accessToken))
                throw new UnauthorizedAccessException("Access token is missing.");

            //default page size
            var pageSize = request.PageSize ?? 5;
            var query = new List<string>{ $"pageNumber={request.PageNumber}", $"pageSize={ pageSize }" };

            if (!string.IsNullOrWhiteSpace(request?.SearchTerm))
            {
                query.Add($"searchTerm={Uri.EscapeDataString(request.SearchTerm)}");
            }
            var url = $"/api/admin/users?{string.Join("&", query)}";
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            response.EnsureSuccessStatusCode();

            var pagedResponse = await response.Content.ReadFromJsonAsync<PagedResponse<UserResponseDTO>>(cancellationToken: cancellationToken);
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
            return new UsersIndexViewModel
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
        }
    }
}
