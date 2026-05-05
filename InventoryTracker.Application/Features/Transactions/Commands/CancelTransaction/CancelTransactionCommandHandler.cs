using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Transactions.DTOs;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Shared.Enums;
using MediatR;

namespace InventoryTracker.Application.Features.Transactions.Commands.CancelTransaction
{
    public class CancelTransactionCommandHandler : IRequestHandler<CancelTransactionCommand, Guid>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ITransactionsRepository _transactionsRepository;
        public CancelTransactionCommandHandler(IAppDbContext context, ICurrentUserService currentUserService, ITransactionsRepository transactionsRepository)
        {
            _context = context;
            _currentUserService = currentUserService;
            _transactionsRepository = transactionsRepository;
        }
        public async Task<Guid> Handle(CancelTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionsRepository.GetTransactionByIdAsync(request.TransactionId, cancellationToken);
            if (transaction == null)
                throw new RecordNotFoundException(nameof(Transaction), request.TransactionId);

            if (transaction.Status != TransactionStatus.Draft)
                throw new BusinessException("Transaction cannot be cancelled due to its status.");

            transaction.Status = TransactionStatus.Cancelled;
            transaction.CancellationReason = request.CancellationReason;
            transaction.CancelledAt = DateTime.UtcNow;
            transaction.CancelledBy = _currentUserService.Email ?? _currentUserService.UserId ?? "system";
            await _context.SaveChangesAsync(cancellationToken);
            //return new TransactionDTO
            //{
            //    TransactionId = transaction.TransactionId,
            //    Type = transaction.Type,
            //    Status = transaction.Status,
            //    TransactionDate = transaction.TransactionDate,
            //    CancelledAt = transaction.CancelledAt,
            //    CancelledBy = transaction.CancelledBy,
            //    CancellationReason = transaction.CancellationReason,
            //    Items = transaction.TransactionItems.Select(i => new TransactionItemDTO
            //    {
            //        TransactionItemId = i.TransactionItemId,
            //        ItemId = i.ItemId,
            //        NameSnapshot = i.NameSnapshot,
            //        SKUSnapshot = i.SKUSnapshot,
            //        UnitOfMeasureSnapshot = i.UnitOfMeasureSnapshot,
            //        UnitCreditValueSnapshot = i.UnitCreditValueSnapshot,
            //        Quantity = i.Quantity
            //    }).ToList()
            //};
            return transaction.TransactionId;
        }
    }
}
