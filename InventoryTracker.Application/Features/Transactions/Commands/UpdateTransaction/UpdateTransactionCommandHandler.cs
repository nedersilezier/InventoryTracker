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

namespace InventoryTracker.Application.Features.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, TransactionDTO?>
    {
        private readonly IAppDbContext _context;
        public UpdateTransactionCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<TransactionDTO?> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions.Include(t => t.TransactionItems).FirstOrDefaultAsync(t => t.TransactionId == request.TransactionId, cancellationToken);
            if(transaction == null)
                throw new RecordNotFoundException(nameof(Transaction), request.TransactionId);
            if(transaction.Status != TransactionStatus.Draft)
                throw new InvalidOperationException("Cannot update due to transaction's status.");

            Client? client = null;
            Warehouse? sourceWarehouse = null;
            Warehouse? destinationWarehouse = null;

            //validate client and warehouses when provided
            if (request.ClientId.HasValue)
            {
                client = await _context.Clients.Where(c => c.IsActive == true && c.ClientId == request.ClientId).FirstOrDefaultAsync(cancellationToken);
                if (client == null)
                    throw new InvalidOperationException($"Client with id {request.ClientId} does not exist or is inactive.");
            }

            if (request.SourceWarehouseId.HasValue)
            {
                sourceWarehouse = await _context.Warehouses.Where(w => w.IsActive == true && w.WarehouseId == request.SourceWarehouseId).FirstOrDefaultAsync(cancellationToken);
                if (sourceWarehouse == null)
                {
                    throw new InvalidOperationException("Source warehouse does not exist or is inactive.");
                }
            }
            if (request.DestinationWarehouseId.HasValue)
            {
                destinationWarehouse = await _context.Warehouses.Where(w => w.IsActive == true && w.WarehouseId == request.DestinationWarehouseId).FirstOrDefaultAsync(cancellationToken);
                if (destinationWarehouse == null)
                {
                    throw new InvalidOperationException("Destination warehouse does not exist or is inactive.");
                }
            }

            //update transaction properties
            transaction.Type = request.Type;
            transaction.TransactionDate = request.TransactionDate;
            transaction.ReferenceNumber = request.ReferenceNumber;
            transaction.Notes = request.Notes;

            //update client and warehouses basef on transaction type
            switch (request.Type)
            {
                case TransactionType.IssueToClient:
                    transaction.ClientId = request.ClientId;
                    transaction.Client = client;

                    transaction.SourceWarehouseId = request.SourceWarehouseId;
                    transaction.SourceWarehouse = sourceWarehouse;
                    transaction.SourceWarehouseNameSnapshot = sourceWarehouse?.Name;

                    transaction.DestinationWarehouseId = null;
                    transaction.DestinationWarehouse = null;
                    transaction.DestinationWarehouseNameSnapshot = null;
                    break;

                case TransactionType.ReturnFromClient:
                    transaction.ClientId = request.ClientId;
                    transaction.Client = client;

                    transaction.DestinationWarehouseId = request.DestinationWarehouseId;
                    transaction.DestinationWarehouse = destinationWarehouse;
                    transaction.DestinationWarehouseNameSnapshot = destinationWarehouse?.Name;

                    transaction.SourceWarehouseId = null;
                    transaction.SourceWarehouse = null;
                    transaction.SourceWarehouseNameSnapshot = null;
                    break;

                case TransactionType.TransferBetweenWarehouses:
                    transaction.SourceWarehouseId = request.SourceWarehouseId;
                    transaction.SourceWarehouse = sourceWarehouse;
                    transaction.SourceWarehouseNameSnapshot = sourceWarehouse?.Name;

                    transaction.DestinationWarehouseId = request.DestinationWarehouseId;
                    transaction.DestinationWarehouse = destinationWarehouse;
                    transaction.DestinationWarehouseNameSnapshot = destinationWarehouse?.Name;

                    transaction.ClientId = null;
                    transaction.Client = null;
                    break;

                case TransactionType.Adjustment:
                    transaction.ClientId = null;
                    transaction.Client = null;

                    if (request.SourceWarehouseId.HasValue)
                    {
                        transaction.SourceWarehouseId = request.SourceWarehouseId;
                        transaction.SourceWarehouse = sourceWarehouse;
                        transaction.SourceWarehouseNameSnapshot = sourceWarehouse?.Name;

                        transaction.DestinationWarehouseId = null;
                        transaction.DestinationWarehouse = null;
                        transaction.DestinationWarehouseNameSnapshot = null;
                    }
                    else
                    {
                        transaction.DestinationWarehouseId = request.DestinationWarehouseId;
                        transaction.DestinationWarehouse = destinationWarehouse;
                        transaction.DestinationWarehouseNameSnapshot = destinationWarehouse?.Name;

                        transaction.SourceWarehouseId = null;
                        transaction.SourceWarehouse = null;
                        transaction.SourceWarehouseNameSnapshot = null;
                    }
                    break;
            }

            //update transaction items if needed
            var oldItemIds = transaction.TransactionItems.Select(ti => ti.ItemId).ToList();
            var newItemIds = request.Items.Select(i => i.ItemId).Distinct().ToList();
            var itemIdsToRemove = oldItemIds.Except(newItemIds).ToList();
            var itemIdsToAdd = newItemIds.Except(oldItemIds).ToList();
            if(itemIdsToRemove.Count > 0)
            {
                var itemsToRemove = transaction.TransactionItems.Where(ti => itemIdsToRemove.Contains(ti.ItemId)).ToList();
                foreach( var itemToRemove in itemsToRemove)
                    transaction.TransactionItems.Remove(itemToRemove);
            }

            var requestItemsDict = request.Items.ToDictionary(i => i.ItemId);
            if (itemIdsToAdd.Count > 0)
            {
                var itemsToAdd = await _context.Items.Where(i => i.IsActive == true && itemIdsToAdd.Contains(i.ItemId)).ToListAsync(cancellationToken);
                if (itemsToAdd.Count != itemIdsToAdd.Count)
                    throw new InvalidOperationException("One or more items do not exist or are inactive.");
                foreach (var itemToAdd in itemsToAdd)
                {
                    var transactionItem = new TransactionItem
                    {
                        TransactionId = transaction.TransactionId,
                        TransactionItemId = Guid.NewGuid(),
                        ItemId = itemToAdd.ItemId,
                        NameSnapshot = itemToAdd.Name,
                        SKUSnapshot = itemToAdd.SKU,
                        UnitOfMeasureSnapshot = itemToAdd.UnitOfMeasure,
                        Quantity = requestItemsDict[itemToAdd.ItemId].Quantity,
                        UnitCreditValueSnapshot = itemToAdd.CreditValue
                    };
                    await _context.TransactionItems.AddAsync(transactionItem, cancellationToken);
                    transaction.TransactionItems.Add(transactionItem);
                }
             }

            var itemsToUpdate = transaction.TransactionItems.Where(ti => ti.Quantity != requestItemsDict[ti.ItemId].Quantity).ToList();
            foreach(var itemToUpdate in itemsToUpdate)
            {
                itemToUpdate.Quantity = requestItemsDict[itemToUpdate.ItemId].Quantity;
            }
            await _context.SaveChangesAsync(cancellationToken);
            return new TransactionDTO
            {
                TransactionId = transaction.TransactionId,
                Type = transaction.Type,
                Status = transaction.Status,
                ClientId = transaction.ClientId,
                ClientName = transaction.Client == null ? null : transaction.Client.Name,
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