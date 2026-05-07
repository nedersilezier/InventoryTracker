using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Transactions;
using InventoryTracker.Contracts.Responses.Transactions;
using InventoryTracker.WebOperator.ViewModels.Transactions;

namespace InventoryTracker.WebOperator.Interfaces
{
    public interface ITransactionsService
    {
        Task<ServiceResult<OperatorTransactionsIndexViewModel>> GetAllAsync(GetTransactionsRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<CreateEditTransactionViewModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ServiceResult<CreateTransactionResponse>> CreateTransactionAsync(CreateTransactionRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<CreateTransactionResponse>> UpdateTransactionAsync(Guid id, UpdateTransactionRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<Guid>> CancelTransactionAsync(Guid id, CancellationToken cancellationToken);
    }
}
