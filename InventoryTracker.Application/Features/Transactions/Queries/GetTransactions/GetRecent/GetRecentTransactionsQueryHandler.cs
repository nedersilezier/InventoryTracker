using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Transactions.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Transactions.Queries.GetTransactions
{
    public class GetRecentTransactionsQueryHandler:IRequestHandler<GetRecentTransactionsQuery, IReadOnlyList<TransactionListDTO>>
    {
        private readonly ITransactionsQueryService _transactionsQueryService;
        public GetRecentTransactionsQueryHandler(ITransactionsQueryService transactionsQueryService)
        {
            _transactionsQueryService = transactionsQueryService;
        }
        public async Task<IReadOnlyList<TransactionListDTO>> Handle(GetRecentTransactionsQuery request, CancellationToken cancellationToken)
        {

            return await _transactionsQueryService.GetRecentTransactionsAsync(request.Count, cancellationToken);
        }
    }
}
