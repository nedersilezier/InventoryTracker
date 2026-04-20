using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Transactions.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Transactions.Queries.GetTransactions
{
    public class GetTransactionsQueryHandler: IRequestHandler<GetTransactionsQuery, PagedResult<TransactionListDTO>>
    {
        private readonly ITransactionsQueryService _transactionsQueryService;
        public GetTransactionsQueryHandler(ITransactionsQueryService transactionsQueryService)
        {
            _transactionsQueryService = transactionsQueryService;
        }
        public async Task<PagedResult<TransactionListDTO>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            var parameters = new GetTransactionsParameters
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                SearchTerm = request.SearchTerm,
                IncludeAdjustments = request.IncludeAdjustments,
                IncludeIssues = request.IncludeIssues,
                IncludeReturns = request.IncludeReturns,
                IncludeTransfers = request.IncludeTransfers
            };
            return await _transactionsQueryService.GetAllTransactionsAsync(parameters, cancellationToken);
        }
    }
}
