using InventoryTracker.Application.Features.Transactions.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Transactions.Commands.CancelTransaction
{
    public class CancelTransactionCommand: IRequest<Guid>
    {
        public Guid TransactionId {  get; set; }
        public string CancellationReason { get; set; } = string.Empty;
    }
}
