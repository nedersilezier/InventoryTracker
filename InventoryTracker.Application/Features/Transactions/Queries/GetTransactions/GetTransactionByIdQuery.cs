using InventoryTracker.Application.Features.Transactions.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Transactions.Queries.GetTransactions
{
    public class GetTransactionByIdQuery: IRequest<TransactionDTO?>
    {
        public Guid TransactionId { get; private set; }
        public GetTransactionByIdQuery(Guid transactionId)
        {
            TransactionId = transactionId;
        }
    }
}
