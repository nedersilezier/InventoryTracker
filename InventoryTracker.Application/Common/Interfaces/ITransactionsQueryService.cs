using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Transactions.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface ITransactionsQueryService
    {
        Task<PagedResult<TransactionListDTO>> GetAllTransactionsAsync(GetTransactionsParameters parameters, CancellationToken cancellationToken);
        Task<TransactionDTO?> GetTransactionByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
