using InventoryTracker.Contracts.Responses.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface ITransactionsService
    {
        Task<IEnumerable<TransactionListDTO>> GetAllTransactionsAsync(CancellationToken cancellationToken);
    }
}
