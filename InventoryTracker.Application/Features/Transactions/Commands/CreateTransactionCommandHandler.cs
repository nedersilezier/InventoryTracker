using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Transactions.DTOs;
using InventoryTracker.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using InventoryTracker.Domain.Enums;

namespace InventoryTracker.Application.Features.Transactions.Commands
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, TransactionDTO>
    {
        private readonly IAppDbContext _context;
        public CreateTransactionCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<TransactionDTO> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            string? sourceWarehouseName = null;
            string? destinationWarehouseName = null;
            //check if items are present
            if (request.Items is null || request.Items.Count == 0)
            {
                throw new InvalidOperationException("Transaction must contain at least one item.");
            }
            // Validate quantities
            if (request.Items.Any(i => i.Quantity < 0))
                throw new InvalidOperationException("Item quantity cannot be negative.");

            // Validate based on transaction type
            switch (request.Type)
            {
                case TransactionType.IssueToClient:
                    if (!request.ClientId.HasValue || !request.SourceWarehouseId.HasValue || request.DestinationWarehouseId.HasValue)
                        throw new InvalidOperationException("IssueToClient requires ClientId and SourceWarehouseId only.");
                    break;

                case TransactionType.ReturnFromClient:
                    if (!request.ClientId.HasValue || !request.DestinationWarehouseId.HasValue || request.SourceWarehouseId.HasValue)
                        throw new InvalidOperationException("ReturnFromClient requires ClientId and DestinationWarehouseId only.");
                    break;

                case TransactionType.TransferBetweenWarehouses:
                    if (!request.SourceWarehouseId.HasValue || !request.DestinationWarehouseId.HasValue || request.ClientId.HasValue)
                        throw new InvalidOperationException("TransferBetweenWarehouses requires source and destination warehouse.");
                    break;

                case TransactionType.Adjustment:
                    if (!request.SourceWarehouseId.HasValue && !request.DestinationWarehouseId.HasValue)
                        throw new InvalidOperationException("Adjustment requires at least one warehouse.");
                    break;
            }
            //validate client and warehouses if provided
            var clientName = await _context.Clients.Where(c => c.ClientId == request.ClientId).Select(c => c.Name).FirstOrDefaultAsync(cancellationToken);
            if (request.ClientId.HasValue && clientName == null)
                throw new InvalidOperationException($"Client with id {request.ClientId} not found");


            if (request.SourceWarehouseId.HasValue)
            {
                sourceWarehouseName = await _context.Warehouses
                    .Where(w => w.WarehouseId == request.SourceWarehouseId)
                    .Select(w => w.Name)
                    .FirstOrDefaultAsync(cancellationToken);
                if (sourceWarehouseName == null)
                {
                    throw new InvalidOperationException("Source warehouse does not exist or is inactive.");
                }
            }
            if (request.DestinationWarehouseId.HasValue)
            {
                destinationWarehouseName = await _context.Warehouses
                    .Where(w => w.WarehouseId == request.DestinationWarehouseId)
                    .Select(w => w.Name)
                    .FirstOrDefaultAsync(cancellationToken);
                if (destinationWarehouseName == null)
                {
                    throw new InvalidOperationException("Destination warehouse does not exist or is inactive.");
                }
            }

            // Get item snapshots
            var itemIds = request.Items.Select(i => i.ItemId).ToList();
            var items = await _context.Items
                .AsNoTracking()
                .Where(i => itemIds.Contains(i.ItemId))
                .ToDictionaryAsync(i => i.ItemId, cancellationToken);

            //validate if all items exist and are active
            if (itemIds.Count != items.Count)
                throw new InvalidOperationException("One or more items do not exist or are inactive.");

            

            // Create transaction
            var transaction = new Transaction
            {
                TransactionId = Guid.NewGuid(),
                Type = request.Type,
                Status = TransactionStatus.Draft,
                ClientId = request.ClientId,
                SourceWarehouseId = request.SourceWarehouseId,
                DestinationWarehouseId = request.DestinationWarehouseId,
                SourceWarehouseNameSnapshot = sourceWarehouseName,
                DestinationWarehouseNameSnapshot = destinationWarehouseName,
                TransactionDate = request.TransactionDate,
                ReferenceNumber = request.ReferenceNumber,
                Notes = request.Notes,
                TransactionItems = request.Items.Select(i =>
                {
                    var item = items[i.ItemId];
                    return new TransactionItem
                    {
                        TransactionItemId = Guid.NewGuid(),
                        ItemId = i.ItemId,
                        NameSnapshot = item.Name,
                        SKUSnapshot = item.SKU,
                        UnitOfMeasureSnapshot = item.UnitOfMeasure,
                        Quantity = i.Quantity,
                        UnitCreditValueSnapshot = item.CreditValue
                    };
                }).ToList()
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            return new TransactionDTO
            {
                TransactionId = transaction.TransactionId,
                Type = transaction.Type,
                Status = transaction.Status,
                ClientId = transaction.ClientId,
                ClientName = clientName,
                SourceWarehouseId = transaction.SourceWarehouseId,
                SourceWarehouseNameSnapshot = transaction.SourceWarehouseNameSnapshot,
                DestinationWarehouseId = transaction.DestinationWarehouseId,
                DestinationWarehouseNameSnapshot = transaction.DestinationWarehouseNameSnapshot,
                TransactionDate = transaction.TransactionDate,
                ReferenceNumber = transaction.ReferenceNumber,
                Notes = transaction.Notes,
                Items = transaction.TransactionItems.Select(ti => new TransactionItemDTO
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
            };
        }
    }
}
