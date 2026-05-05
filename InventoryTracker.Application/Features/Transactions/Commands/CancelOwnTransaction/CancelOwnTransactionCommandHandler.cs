using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Shared.Enums;
using MediatR;

namespace InventoryTracker.Application.Features.Transactions.Commands.CancelOwnTransaction
{
    public class CancelOwnTransactionCommandHandler : IRequestHandler<CancelOwnTransactionCommand, Guid>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ITransactionsRepository _transactionsRepository;
        public CancelOwnTransactionCommandHandler(IAppDbContext context, ICurrentUserService currentUserService, ITransactionsRepository transactionsRepository)
        {
            _context = context;
            _currentUserService = currentUserService;
            _transactionsRepository = transactionsRepository;
        }
        public async Task<Guid> Handle(CancelOwnTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionsRepository.GetTransactionByIdAsync(request.TransactionId, cancellationToken);
            if (transaction == null)
                throw new RecordNotFoundException(nameof(Transaction), request.TransactionId);

            if (transaction.Status != TransactionStatus.Draft)
                throw new BusinessException("Transaction cannot be cancelled due to its status.");
            var currentUserName = _currentUserService.Email;

            if (transaction.CreatedBy != currentUserName)
                throw new BusinessException("Transaction cannot be cancelled due to its ownership.");

            transaction.Status = TransactionStatus.Cancelled;
            transaction.CancellationReason = request.CancellationReason;
            transaction.CancelledAt = DateTime.UtcNow;
            transaction.CancelledBy = currentUserName;
            await _context.SaveChangesAsync(cancellationToken);

            return transaction.TransactionId;
        }
    }
}
