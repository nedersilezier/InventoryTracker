using InventoryTracker.Application.Features.Transactions.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Transactions.Queries.GetTransactions
{
    public class GetTransactionQuery: IRequest<List<TransactionDTO>>
    {
    }
}
