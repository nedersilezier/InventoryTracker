using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Transactions.DTOs;
using InventoryTracker.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using InventoryTracker.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using InventoryTracker.Application.Common.Exceptions;

namespace InventoryTracker.Application.Features.Transactions.Commands.CreateTransaction
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
            Client? client = null;
            Warehouse? sourceWarehouse = null;
            Warehouse? destinationWarehouse = null;

            //validate client and warehouses if provided
            if (request.ClientId.HasValue)
            {
                client = await _context.Clients.Where(c => c.IsActive == true && c.ClientId == request.ClientId).FirstOrDefaultAsync(cancellationToken);
                if(client == null)
                    throw new BusinessException($"Client with id {request.ClientId} does not exist or is inactive.");
            }
                
            if (request.SourceWarehouseId.HasValue)
            {
                sourceWarehouse = await _context.Warehouses.Where(w => w.IsActive == true && w.WarehouseId == request.SourceWarehouseId).FirstOrDefaultAsync(cancellationToken);
                if (sourceWarehouse == null)
                {
                    throw new BusinessException($"Source warehouse with id {request.SourceWarehouseId} does not exist or is inactive.");
                }
            }
            if (request.DestinationWarehouseId.HasValue)
            {
                destinationWarehouse = await _context.Warehouses.Where(w => w.IsActive == true && w.WarehouseId == request.DestinationWarehouseId).FirstOrDefaultAsync(cancellationToken);
                if (destinationWarehouse == null)
                {
                    throw new BusinessException($"Destination warehouse with id {request.DestinationWarehouseId} does not exist or is inactive.");
                }
            }

            // Get item snapshots
            var itemIds = request.Items.Select(i => i.ItemId).Distinct().ToList();
            var items = await _context.Items
                .AsNoTracking()
                .Where(i => i.IsActive == true && itemIds.Contains(i.ItemId))
                .ToDictionaryAsync(i => i.ItemId, cancellationToken);

            //validate if all items exist and are active
            if (itemIds.Count != items.Count)
                throw new BusinessException("One or more items do not exist or are inactive.");

            // Create transaction
            var transaction = new Transaction
            {
                TransactionId = Guid.NewGuid(),
                Type = request.Type,
                Status = TransactionStatus.Draft,
                Client = client,
                ClientId = request.ClientId,
                
                SourceWarehouse = sourceWarehouse,
                SourceWarehouseId = request.SourceWarehouseId,
                SourceWarehouseNameSnapshot = sourceWarehouse == null ? null : sourceWarehouse.Name,

                DestinationWarehouse = destinationWarehouse,
                DestinationWarehouseId = request.DestinationWarehouseId,
                DestinationWarehouseNameSnapshot = destinationWarehouse == null ?  null : destinationWarehouse.Name,

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
                ClientName = client == null ? null : client.Name,
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