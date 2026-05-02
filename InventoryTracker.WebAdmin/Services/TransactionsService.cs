using InventoryTracker.APIClient;
using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Transactions;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Transactions;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Transactions;

namespace InventoryTracker.WebAdmin.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ApiHttpClient _apiClient;

        public TransactionsService(ApiHttpClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<ServiceResult<TransactionsIndexViewModel>> GetAllAsync(GetTransactionsRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

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
            var result = await _apiClient.GetAsync<PagedResponse<TransactionsResponseDTO>>(url, "Failed to load transactions.", cancellationToken);
            if (!result.Success)
                return ServiceResult<TransactionsIndexViewModel>.Fail(result.ErrorMessage, result.ValidationErrors, result.StatusCode);

            var pagedResponse = result.Data!;
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
            var viewModel = new TransactionsIndexViewModel
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
            return ServiceResult<TransactionsIndexViewModel>.Ok(viewModel);
        }
        public async Task<ServiceResult<IEnumerable<TransactionListDTO>>> GetRecentTransactionsAsync(int count, CancellationToken cancellationToken)
        {
            var url = $"api/admin/transactions/recent/{count}";


            var result = await _apiClient.GetAsync<IEnumerable<TransactionListDTO>>(url, "Failed to load transactions.", cancellationToken);
            if (!result.Success)
                return ServiceResult<IEnumerable<TransactionListDTO>>.Fail(result.ErrorMessage, result.ValidationErrors, result.StatusCode);
            var response = result.Data!;

            return ServiceResult<IEnumerable<TransactionListDTO>>.Ok(response);
        }
        public async Task<ServiceResult<CreateEditTransactionViewModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetAsync<TransactionForEditDTO>($"/api/admin/transactions/{id}", "Failed to load transaction.", cancellationToken);

            if (!result.Success)
                return ServiceResult<CreateEditTransactionViewModel>.Fail(result.ErrorMessage, result.ValidationErrors, result.StatusCode);

            var transaction = result.Data!;

            var vm = new CreateEditTransactionViewModel
            {
                TransactionId = transaction.TransactionId,
                Type = transaction.Type,
                ClientId = transaction.ClientId,
                SourceWarehouseId = transaction.SourceWarehouseId,
                DestinationWarehouseId = transaction.DestinationWarehouseId,
                TransactionDate = transaction.TransactionDate,
                ReferenceNumber = transaction.ReferenceNumber,
                Notes = transaction.Notes,
                SelectedItems = transaction.Items.Select(ti => new CreateEditTransactionItemViewModel
                {
                    ItemId = ti.ItemId,
                    Name = ti.NameSnapshot,
                    Quantity = ti.Quantity
                }).ToList()
            };

            return ServiceResult<CreateEditTransactionViewModel>.Ok(vm);
        }
        public async Task<ServiceResult<CreateTransactionResponse>> CreateTransactionAsync(CreateTransactionRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            return await _apiClient.PostAsync<CreateTransactionResponse>("/api/admin/transactions", request, "Failed to create transaction.", cancellationToken);
        }
        public async Task<ServiceResult<CreateTransactionResponse>> UpdateTransactionAsync(Guid id, UpdateTransactionRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            return await _apiClient.PutAsync<CreateTransactionResponse>($"/api/admin/transactions/{id}", request, "Failed to update transaction.", cancellationToken);
        }
        public async Task<ServiceResult<Guid>> ApproveTransactionAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _apiClient.PatchAsync<Guid>($"/api/admin/transactions/{id}/approve", null, "Failed to approve transaction.", cancellationToken);
        }
        public async Task<ServiceResult<Guid>> CancelTransactionAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _apiClient.PatchAsync<Guid>($"/api/admin/transactions/{id}/cancel", null, "Failed to cancel transaction.", cancellationToken);
        }
    }
}
