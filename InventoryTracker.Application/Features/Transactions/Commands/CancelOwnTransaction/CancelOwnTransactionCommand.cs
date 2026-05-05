using MediatR;

namespace InventoryTracker.Application.Features.Transactions.Commands.CancelOwnTransaction
{
    public class CancelOwnTransactionCommand : IRequest<Guid>
    {
        public Guid TransactionId { get; set; }
        public string CancellationReason { get; set; } = string.Empty;
    }
}
