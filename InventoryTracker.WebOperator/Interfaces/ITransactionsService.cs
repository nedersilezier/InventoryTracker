using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Transactions;
using InventoryTracker.WebOperator.ViewModels.Transactions;

namespace InventoryTracker.WebOperator.Interfaces
{
    public interface ITransactionsService
    {
        Task<ServiceResult<OperatorTransactionsIndexViewModel>> GetAllAsync(GetTransactionsRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<Guid>> CancelTransactionAsync(Guid id, CancellationToken cancellationToken);
    }
}
