using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Transactions.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Transactions.Queries.GetTransactions.GetUsersDrafts
{
    public class GetUsersDraftsQueryHandler: IRequestHandler<GetUsersDraftsQuery, PagedResult<TransactionListDTO>>
    {
        private readonly ITransactionsQueryService _transactionsQueryService;
        private readonly ICurrentUserService _currentUserService;
        public GetUsersDraftsQueryHandler(ITransactionsQueryService transactionsQueryService, ICurrentUserService currentUserService)
        {
            _transactionsQueryService = transactionsQueryService;
            _currentUserService = currentUserService;
        }
        public async Task<PagedResult<TransactionListDTO>> Handle(GetUsersDraftsQuery request, CancellationToken cancellationToken)
        {
            var username = _currentUserService.Email;
            var parameters = new GetUsersDraftsParameters
            {
                UserName = username ?? string.Empty,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                SearchTerm = request.SearchTerm?.Trim(),
                IncludeAdjustments = request.IncludeAdjustments,
                IncludeIssues = request.IncludeIssues,
                IncludeReturns = request.IncludeReturns,
                IncludeTransfers = request.IncludeTransfers,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo.HasValue ? request.DateTo.Value.AddDays(1) : null,
            };
            return await _transactionsQueryService.GetCurrentUsersDraftsAsync(parameters, cancellationToken);
        }
    }
}
