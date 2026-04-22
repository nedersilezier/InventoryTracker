using InventoryTracker.Contracts.Requests.Transactions;
using InventoryTracker.Contracts.Responses.Transactions;
using InventoryTracker.WebAdmin.ViewModels.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface ITransactionsService
    {
        Task<TransactionsIndexViewModel> GetAllTransactionsAsync(GetTransactionsRequest request, CancellationToken cancellationToken);
        Task<IEnumerable<TransactionListDTO>> GetRecentTransactionsAsync(int count, CancellationToken cancellationToken);
    }
}
