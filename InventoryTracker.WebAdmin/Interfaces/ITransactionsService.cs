using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Transactions;
using InventoryTracker.Contracts.Responses.Transactions;
using InventoryTracker.WebAdmin.ViewModels.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface ITransactionsService
    {
        Task<ServiceResult<TransactionsIndexViewModel>> GetAllAsync(GetTransactionsRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<CreateEditTransactionViewModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ServiceResult<CreateTransactionResponse>> CreateTransactionAsync(CreateTransactionRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<CreateTransactionResponse>> UpdateTransactionAsync(Guid id, UpdateTransactionRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<IEnumerable<TransactionListDTO>>> GetRecentTransactionsAsync(int count, CancellationToken cancellationToken);
        Task<ServiceResult<Guid>> ApproveTransactionAsync(Guid id, CancellationToken cancellationToken);
        Task<ServiceResult<Guid>> CancelTransactionAsync(Guid id, CancellationToken cancellationToken);
    }
}
