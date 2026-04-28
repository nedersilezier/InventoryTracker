using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Domain.Entities;
using MediatR;
using InventoryTracker.Shared.Enums;
using InventoryTracker.Application.Common.Exceptions;

namespace InventoryTracker.Application.Features.Transactions.Commands.CreateTransaction
{
    // TODO:
    // - Add TransactionType: Settlement
    // - When items are not returned but paid for, create settlement transaction
    // - Ensure Client.Saldo is always derived from transactions

    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Guid>
    {
        private readonly IAppDbContext _context;
        private readonly IClientsRepository _clientsRepository;
        private readonly IWarehousesRepository _warehousesRepository;
        private readonly IItemsRepository _itemsRepository;
        private readonly ITransactionsRepository _transactionsRepository;
        public CreateTransactionCommandHandler(IAppDbContext context, IClientsRepository clientsRepository, IWarehousesRepository warehousesRepository, IItemsRepository itemsRepository, ITransactionsRepository transactionsRepository)
        {
            _context = context;
            _clientsRepository = clientsRepository;
            _warehousesRepository = warehousesRepository;
            _itemsRepository = itemsRepository;
            _transactionsRepository = transactionsRepository;
        }

        public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            Client? client = null;
            Warehouse? sourceWarehouse = null;
            Warehouse? destinationWarehouse = null;
            
            //validate client and warehouses if provided
            if (request.ClientId.HasValue)
            {
                client = await _clientsRepository.GetActiveClientByIdAsync(request.ClientId.Value, cancellationToken);
                if (client == null)
                    throw new BusinessException($"Client with id {request.ClientId} does not exist or is inactive.");
            }
                
            if (request.SourceWarehouseId.HasValue)
            {
                sourceWarehouse = await _warehousesRepository.GetActiveWarehouseByIdAsync(request.SourceWarehouseId.Value, cancellationToken);
                if (sourceWarehouse == null)
                    throw new BusinessException($"Source warehouse with id {request.SourceWarehouseId} does not exist or is inactive.");
            }
            if (request.DestinationWarehouseId.HasValue)
            {
                destinationWarehouse = await _warehousesRepository.GetActiveWarehouseByIdAsync(request.DestinationWarehouseId.Value, cancellationToken);
                if (destinationWarehouse == null)
                    throw new BusinessException($"Destination warehouse with id {request.DestinationWarehouseId} does not exist or is inactive.");
            }

            // Get item snapshots
            var itemIds = request.Items.Select(i => i.ItemId).Distinct().ToList();
            var items = await _itemsRepository.GetActiveItemsByIdsAsync(itemIds, cancellationToken);
            var itemsDict = items.ToDictionary(i => i.ItemId, i => i);

            //validate if all items exist and are active
            if (itemIds.Count != items.Count())
                throw new BusinessException("One or more items do not exist or are inactive.");

            // Create transaction
            var transaction = new Transaction
            {
                TransactionId = Guid.NewGuid(),
                Type = request.Type,
                Status = TransactionStatus.Draft,
                ClientId = request.ClientId,
                
                SourceWarehouseId = request.SourceWarehouseId,
                SourceWarehouseNameSnapshot = sourceWarehouse == null ? null : sourceWarehouse.Name,

                DestinationWarehouseId = request.DestinationWarehouseId,
                DestinationWarehouseNameSnapshot = destinationWarehouse == null ?  null : destinationWarehouse.Name,

                TransactionDate = request.TransactionDate,
                ReferenceNumber = request.ReferenceNumber,
                Notes = request.Notes,
                TransactionItems = request.Items.Select(i =>
                {
                    var item = itemsDict[i.ItemId];
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

            await _transactionsRepository.AddTransaction(transaction);
            await _context.SaveChangesAsync(cancellationToken);
            return transaction.TransactionId;
        }
    }
}