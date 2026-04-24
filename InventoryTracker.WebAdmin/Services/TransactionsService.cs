using InventoryTracker.Contracts.Requests.Transactions;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Countries;
using InventoryTracker.Contracts.Responses.Transactions;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.Countries;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Transactions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace InventoryTracker.WebAdmin.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionsService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<TransactionsIndexViewModel> GetAllTransactionsAsync(GetTransactionsRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];
            if (string.IsNullOrEmpty(accessToken))
                throw new UnauthorizedAccessException("Access token is missing.");

            var pageSize = request.PageSize == 0 ? 5 : request.PageSize;
            var query = new List<string>
            { 
                $"pageNumber={request.PageNumber}", 
                $"pageSize={pageSize}", 
                $"includeAdjustments={request.IncludeAdjustments}", 
                $"includeTransfers={request.IncludeTransfers}", 
                $"includeIssues={request.IncludeIssues}", 
                $"includeReturns={request.IncludeReturns}"
            };
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query.Add($"searchTerm={Uri.EscapeDataString(request.SearchTerm)}");
            }

            if (request.DateFrom.HasValue)
            {
                query.Add($"dateFrom={request.DateFrom.Value:yyyy-MM-dd}");
            }

            if (request.DateTo.HasValue)
            {
                query.Add($"dateTo={request.DateTo.Value:yyyy-MM-dd}");
            }
            var url = $"/api/admin/transactions?{string.Join("&", query)}";
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            response.EnsureSuccessStatusCode();
            var pagedResponse = await response.Content.ReadFromJsonAsync<PagedResponse<TransactionsResponseDTO>>(cancellationToken: cancellationToken);
            var transactions = pagedResponse?.Items.Select(transaction => new TransactionListItemViewModel
            {
                TransactionId = transaction.TransactionId,
                Type = transaction.Type,
                Status = transaction.Status,
                TransactionDate = transaction.TransactionDate,
                FromDisplay = transaction.FromDisplay ?? string.Empty,
                ToDisplay = transaction.ToDisplay ?? string.Empty,
                ReferenceNumber = transaction.ReferenceNumber,
                StatusDisplay = transaction.StatusName,
                TypeDisplay = transaction.TypeName,
            }).ToList() ?? new List<TransactionListItemViewModel>();
            
            var routeValues = new Dictionary<string, string?>
            {
                ["PageSize"] = pageSize.ToString(),
                ["IncludeAdjustments"] = request.IncludeAdjustments.ToString(),
                ["IncludeTransfers"] = request.IncludeTransfers.ToString(),
                ["IncludeIssues"] = request.IncludeIssues.ToString(),
                ["IncludeReturns"] = request.IncludeReturns.ToString()
            };
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                routeValues["SearchTerm"] = request.SearchTerm;
            }
            if (request.DateFrom.HasValue)
            {
                routeValues["DateFrom"] = request.DateFrom.Value.ToString("yyyy-MM-dd");
            }

            if (request.DateTo.HasValue)
            {
                routeValues["DateTo"] = request.DateTo.Value.ToString("yyyy-MM-dd");
            }
            return new TransactionsIndexViewModel
            {
                Transactions = transactions,
                TotalCount = pagedResponse?.TotalCount ?? 0,
                DraftCount = transactions.Count(t => t.Status == Shared.Enums.TransactionStatus.Draft),
                Filters = new TransactionFiltersViewModel
                {
                    SearchTerm = request.SearchTerm,
                    IncludeAdjustments = request.IncludeAdjustments,
                    IncludeTransfers = request.IncludeTransfers,
                    IncludeIssues = request.IncludeIssues,
                    IncludeReturns = request.IncludeReturns,
                    DateFrom = request.DateFrom,
                    DateTo = request.DateTo,
                },
                TableFooter = new TableFooterViewModel
                {
                    DisplayedCount = transactions.Count,
                    TotalCount = pagedResponse?.TotalCount ?? 0,
                    EntityName = "transactions",
                    Pagination = new PaginationViewModel
                    {
                        CurrentPage = pagedResponse?.PageNumber ?? 1,
                        TotalPages = pagedResponse?.TotalPages ?? 1,
                        PageSize = pagedResponse?.PageSize ?? 5,
                        Controller = "Transactions",
                        Action = "Index",
                        RouteValues = routeValues
                    }
                }
            };
        }
        public async Task<IEnumerable<TransactionListDTO>> GetRecentTransactionsAsync(int count, CancellationToken cancellationToken)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];
            if (string.IsNullOrEmpty(accessToken))
                throw new UnauthorizedAccessException("Access token is missing.");

            using var request = new HttpRequestMessage(HttpMethod.Get, $"api/admin/transactions/recent/{count}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<TransactionListDTO>>(cancellationToken: cancellationToken)
                   ?? new List<TransactionListDTO>();
        }
    }
}
