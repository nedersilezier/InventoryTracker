using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Transactions.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Transactions.Queries.GetTransactions
{
    public class GetTransactionByIdQueryHandler: IRequestHandler<GetTransactionByIdQuery, TransactionDTO?>
    {
        private readonly IAppDbContext _context;
        public GetTransactionByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<TransactionDTO?> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions
            .AsNoTracking()
            .Include(t => t.TransactionItems)
            .Where(t => t.TransactionId == request.TransactionId)
            .Select(t => new TransactionDTO
            {
                TransactionId = t.TransactionId,
                Type = t.Type,
                Status = t.Status,
                ClientId = t.ClientId,
                ClientName = t.Client != null ? t.Client.Name : null,
                SourceWarehouseId = t.SourceWarehouseId,
                SourceWarehouseNameSnapshot = t.SourceWarehouseNameSnapshot,
                DestinationWarehouseId = t.DestinationWarehouseId,
                DestinationWarehouseNameSnapshot = t.DestinationWarehouseNameSnapshot,
                TransactionDate = t.TransactionDate,
                ReferenceNumber = t.ReferenceNumber,
                Notes = t.Notes,
                CancelledAt = t.CancelledAt,
                CancelledBy = t.CancelledBy,
                CancellationReason = t.CancellationReason,
                Items = t.TransactionItems.Select(ti => new TransactionItemDTO
                {
                    TransactionItemId = ti.TransactionItemId,
                    ItemId = ti.ItemId,
                    NameSnapshot = ti.NameSnapshot,
                    SKUSnapshot = ti.SKUSnapshot,
                    UnitOfMeasureSnapshot = ti.UnitOfMeasureSnapshot,
                    Quantity = ti.Quantity,
                    UnitCreditValueSnapshot = ti.UnitCreditValueSnapshot,
                    TotalCreditValue = ti.TotalCreditValue
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
            if(transaction == null)
                throw new KeyNotFoundException($"Transaction with ID {request.TransactionId} not found.");
            return transaction;
        }
    }
}
