using InventoryTracker.APIClient;
using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Transactions;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Transactions;
using InventoryTracker.WebOperator.Interfaces;
using InventoryTracker.WebOperator.ViewModels.Transactions;
using InventoryTracker.WebOperator.ViewModels;

namespace InventoryTracker.WebOperator.Services
{

    //TODO: introduce mappers, remove duplicate code
    public class TransactionsService : ITransactionsService
    {
        private readonly ApiHttpClient _apiClient;

        public TransactionsService(ApiHttpClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<ServiceResult<OperatorTransactionsIndexViewModel>> GetAllAsync(GetTransactionsRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var pageSize = request.PageSize == 0 ? 12 : request.PageSize;
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
                var utcFrom = DateTime.SpecifyKind(request.DateFrom.Value.Date, DateTimeKind.Local).ToUniversalTime();
                query.Add($"dateFrom={Uri.EscapeDataString(utcFrom.ToString("O"))}");
            }

            if (request.DateTo.HasValue)
            {
                var utcTo = DateTime.SpecifyKind(request.DateTo.Value.Date, DateTimeKind.Local).ToUniversalTime();
                query.Add($"dateTo={Uri.EscapeDataString(utcTo.ToString("O"))}");
            }
            var url = $"/api/user/transactions?{string.Join("&", query)}";
            var result = await _apiClient.GetAsync<PagedResponse<TransactionsResponseDTO>>(url, "Failed to load transactions.", cancellationToken);
            if (!result.Success)
                return ServiceResult<OperatorTransactionsIndexViewModel>.Fail(result.ErrorMessage, result.ValidationErrors, result.StatusCode);

            var pagedResponse = result.Data!;
            var transactions = pagedResponse?.Items.Select(transaction => new TransactionCardViewModel
            {
                TransactionId = transaction.TransactionId,
                Type = transaction.Type,
                Status = transaction.Status,
                TransactionDate = transaction.TransactionDate,
                FromDisplay = transaction.FromDisplay ?? string.Empty,
                ToDisplay = transaction.ToDisplay ?? string.Empty,
                ReferenceNumber = transaction.ReferenceNumber ?? string.Empty
            }).ToList() ?? new List<TransactionCardViewModel>();

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
            var viewModel = new OperatorTransactionsIndexViewModel
            {
                Transactions = transactions,
                TotalCount = pagedResponse?.TotalCount ?? 0,
                DisplayedCount = transactions.Count,
                Action = "Index",
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
                Pagination = new PaginationViewModel
                {
                    CurrentPage = pagedResponse?.PageNumber ?? 1,
                    TotalPages = pagedResponse?.TotalPages ?? 1,
                    PageSize = pagedResponse?.PageSize ?? 5,
                    Controller = "Transactions",
                    Action = "Index",
                    RouteValues = routeValues
                }
            };
            return ServiceResult<OperatorTransactionsIndexViewModel>.Ok(viewModel);
        }
        public async Task<ServiceResult<CreateEditTransactionViewModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetAsync<TransactionForEditDTO>($"/api/user/transactions/{id}", "Failed to load transaction.", cancellationToken);

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
        public async Task<ServiceResult<OperatorTransactionsIndexViewModel>> GetUsersDraftsAsync(GetTransactionsRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var pageSize = request.PageSize == 0 ? 12 : request.PageSize;
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
                var utcFrom = DateTime.SpecifyKind(request.DateFrom.Value.Date, DateTimeKind.Local).ToUniversalTime();
                query.Add($"dateFrom={Uri.EscapeDataString(utcFrom.ToString("O"))}");
            }

            if (request.DateTo.HasValue)
            {
                var utcTo = DateTime.SpecifyKind(request.DateTo.Value.Date, DateTimeKind.Local).ToUniversalTime();
                query.Add($"dateTo={Uri.EscapeDataString(utcTo.ToString("O"))}");
            }
            var url = $"/api/user/transactions/drafts?{string.Join("&", query)}";
            var result = await _apiClient.GetAsync<PagedResponse<TransactionsResponseDTO>>(url, "Failed to load drafts.", cancellationToken);
            if (!result.Success)
                return ServiceResult<OperatorTransactionsIndexViewModel>.Fail(result.ErrorMessage, result.ValidationErrors, result.StatusCode);

            var pagedResponse = result.Data!;
            var transactions = pagedResponse?.Items.Select(transaction => new TransactionCardViewModel
            {
                TransactionId = transaction.TransactionId,
                Type = transaction.Type,
                Status = transaction.Status,
                TransactionDate = transaction.TransactionDate,
                FromDisplay = transaction.FromDisplay ?? string.Empty,
                ToDisplay = transaction.ToDisplay ?? string.Empty,
                ReferenceNumber = transaction.ReferenceNumber ?? string.Empty
            }).ToList() ?? new List<TransactionCardViewModel>();

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
            var viewModel = new OperatorTransactionsIndexViewModel
            {
                Transactions = transactions,
                TotalCount = pagedResponse?.TotalCount ?? 0,
                DisplayedCount = transactions.Count,
                Action = "Drafts",
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
                Pagination = new PaginationViewModel
                {
                    CurrentPage = pagedResponse?.PageNumber ?? 1,
                    TotalPages = pagedResponse?.TotalPages ?? 1,
                    PageSize = pagedResponse?.PageSize ?? 5,
                    Controller = "Transactions",
                    Action = "Drafts",
                    RouteValues = routeValues
                }
            };
            return ServiceResult<OperatorTransactionsIndexViewModel>.Ok(viewModel);
        }
        public async Task<ServiceResult<CreateTransactionResponse>> CreateTransactionAsync(CreateTransactionRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            return await _apiClient.PostAsync<CreateTransactionResponse>("/api/user/transactions", request, "Failed to create transaction.", cancellationToken);
        }
        public async Task<ServiceResult<CreateTransactionResponse>> UpdateTransactionAsync(Guid id, UpdateTransactionRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            return await _apiClient.PutAsync<CreateTransactionResponse>($"/api/user/transactions/{id}", request, "Failed to update transaction.", cancellationToken);
        }
        public async Task<ServiceResult<Guid>> CancelTransactionAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _apiClient.PatchAsync<Guid>($"/api/user/transactions/{id}/cancel", null, "Failed to cancel transaction.", cancellationToken);
        }
    }
}
