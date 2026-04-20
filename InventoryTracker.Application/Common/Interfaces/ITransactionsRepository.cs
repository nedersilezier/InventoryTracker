using InventoryTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface ITransactionsRepository
    {
        Task AddTransaction(Transaction transaction);

        Task AddTransactionItem(TransactionItem transactionItem);
        Task<Transaction?> GetTransactionWithItemsByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
