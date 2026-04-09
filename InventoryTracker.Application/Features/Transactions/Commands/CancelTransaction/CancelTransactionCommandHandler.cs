using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Transactions.DTOs;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Transactions.Commands.CancelTransaction
{
    public class CancelTransactionCommandHandler : IRequestHandler<CancelTransactionCommand, TransactionDTO?>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public CancelTransactionCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<TransactionDTO?> Handle(CancelTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions.Include(t => t.TransactionItems).FirstOrDefaultAsync(t => t.TransactionId == request.TransactionId, cancellationToken);
            if (transaction == null)
                throw new RecordNotFoundException(nameof(Transaction), request.TransactionId);

            if (transaction.Status != TransactionStatus.Draft)
                throw new InvalidOperationException("Transaction cannot be cancelled due to its status.");

            transaction.Status = TransactionStatus.Cancelled;
            transaction.CancellationReason = request.CancellationReason;
            transaction.CancelledAt = DateTime.UtcNow;
            transaction.CancelledBy = _currentUserService.Email ?? _currentUserService.UserId ?? "system";
            await _context.SaveChangesAsync(cancellationToken);
            return new TransactionDTO
            {
                TransactionId = transaction.TransactionId,
                Type = transaction.Type,
                Status = transaction.Status,
                TransactionDate = transaction.TransactionDate,
                CancelledAt = transaction.CancelledAt,
                CancelledBy = transaction.CancelledBy,
                CancellationReason = transaction.CancellationReason,
                Items = transaction.TransactionItems.Select(i => new TransactionItemDTO
                {
                    TransactionItemId = i.TransactionItemId,
                    ItemId = i.ItemId,
                    NameSnapshot = i.NameSnapshot,
                    SKUSnapshot = i.SKUSnapshot,
                    UnitOfMeasureSnapshot = i.UnitOfMeasureSnapshot,
                    UnitCreditValueSnapshot = i.UnitCreditValueSnapshot,
                    Quantity = i.Quantity
                }).ToList()
            };
        }
    }
}
