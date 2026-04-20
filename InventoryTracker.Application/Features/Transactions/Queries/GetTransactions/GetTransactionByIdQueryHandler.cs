using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Transactions.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Transactions.Queries.GetTransactions
{
    public class GetTransactionByIdQueryHandler: IRequestHandler<GetTransactionByIdQuery, TransactionDTO?>
    {
        private readonly ITransactionsQueryService _transactionsQueryService;
        public GetTransactionByIdQueryHandler(ITransactionsQueryService transactionsQueryService)
        {
            _transactionsQueryService = transactionsQueryService;
        }
        public async Task<TransactionDTO?> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionsQueryService.GetTransactionByIdAsync(request.TransactionId, cancellationToken);
            if (transaction == null)
                throw new KeyNotFoundException($"Transaction with ID {request.TransactionId} not found.");
            return transaction;
        }
    }
}
