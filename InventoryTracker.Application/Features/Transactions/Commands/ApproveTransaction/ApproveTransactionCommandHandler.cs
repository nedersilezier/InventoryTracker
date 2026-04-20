using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Transactions.DTOs;
using MediatR;
using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Shared.Enums;

namespace InventoryTracker.Application.Features.Transactions.Commands.ApproveTransaction
{
    public class ApproveTransactionCommandHandler : IRequestHandler<ApproveTransactionCommand, Guid>
    {
        private readonly IAppDbContext _context;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IStocksRepository _stocksRepository;
        public ApproveTransactionCommandHandler(IAppDbContext context, ITransactionsRepository transactionsRepository, IStocksRepository stocksRepository)
        {
            _context = context;
            _transactionsRepository = transactionsRepository;
            _stocksRepository = stocksRepository;
        }
        public async Task<Guid> Handle(ApproveTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionsRepository.GetTransactionWithItemsByIdAsync(request.TransactionId, cancellationToken);
            //var transaction = await _context.Transactions.Include(t => t.TransactionItems).FirstOrDefaultAsync(t => t.TransactionId == request.TransactionId, cancellationToken);
            if (transaction == null)
                throw new RecordNotFoundException(nameof(Transaction), request.TransactionId);

            if (transaction.Status != TransactionStatus.Draft)
                throw new BusinessException("Transaction cannot be approved due to its status.");

            // update stock levels based on transaction type and items
            var itemIds = transaction.TransactionItems.Select(i => i.ItemId).ToList();

            //helper query
            var stocks = await _stocksRepository.GetStocksByItemIdsAsync(itemIds, cancellationToken);
            IQueryable<Stock> stocksQuery = stocks.AsQueryable();

            //IQueryable<Stock> stocksQuery = _context.Stocks.Where(s => itemIds.Contains(s.ItemId));

            switch (transaction.Type)
            {
                case TransactionType.Adjustment:
                    if (transaction.SourceWarehouseId == null)
                        throw new BusinessException("No source warehouse bound to the transaction.");

                    var stocksToAdjust = stocksQuery.Where(s => s.WarehouseId == transaction.SourceWarehouseId).ToList();
                    //var stocksToAdjust = await stocksQuery.Where(s => s.WarehouseId == transaction.SourceWarehouseId).ToListAsync(cancellationToken);
                    foreach (var item in transaction.TransactionItems)
                    {
                        var stock = stocksToAdjust.FirstOrDefault(s => s.ItemId == item.ItemId);
                        if (stock == null)
                        {
                            stock = new Stock
                            {
                                StockId = Guid.NewGuid(),
                                WarehouseId = transaction.SourceWarehouseId.Value,
                                ItemId = item.ItemId,
                                Quantity = 0
                            };
                            stocksToAdjust.Add(stock);
                           await _stocksRepository.AddStock(stock);
                            //_context.Stocks.Add(stock);
                        }
                        var quantityChange = stock.Quantity + item.Quantity;
                        if (quantityChange < 0)
                            throw new BusinessException($"Insufficient stock for item {item.ItemId} to adjust.");

                        stock.Quantity = quantityChange;
                    }
                    break;

                case TransactionType.IssueToClient:
                    if (transaction.SourceWarehouseId == null)
                        throw new BusinessException("No source warehouse bound to the transaction.");

                    var stocksToSend = stocksQuery.Where(s => s.WarehouseId == transaction.SourceWarehouseId).ToList();
                    //var stocksToSend = await stocksQuery.Where(s => s.WarehouseId == transaction.SourceWarehouseId).ToListAsync(cancellationToken);
                    foreach (var item in transaction.TransactionItems)
                    {
                        var stock = stocksToSend.FirstOrDefault(s => s.ItemId == item.ItemId);
                        if (stock == null)
                            throw new BusinessException($"No stock record found for item {item.ItemId} in the source warehouse.");
                        var quantityChange = stock.Quantity - item.Quantity;
                        if (quantityChange < 0)
                            throw new BusinessException($"Insufficient stock for item {item.ItemId} to issue to client.");
                        stock.Quantity = quantityChange;
                    }
                    break;

                case TransactionType.TransferBetweenWarehouses:
                    if (transaction.SourceWarehouseId == null || transaction.DestinationWarehouseId == null)
                        throw new BusinessException("Source or destination warehouse not bound to the transaction.");

                    var sourceWarehouseStocks = stocksQuery.Where(s => s.WarehouseId == transaction.SourceWarehouseId).ToList();
                    //var sourceWarehouseStocks = await stocksQuery.Where(s => s.WarehouseId == transaction.SourceWarehouseId).ToListAsync(cancellationToken);
                    var destinationWarehouseStocks = stocksQuery.Where(s => s.WarehouseId == transaction.DestinationWarehouseId).ToList();
                    //var destinationWarehouseStocks = await stocksQuery.Where(s => s.WarehouseId == transaction.DestinationWarehouseId).ToListAsync(cancellationToken);

                    foreach (var item in transaction.TransactionItems)
                    {
                        var sourceStock = sourceWarehouseStocks.FirstOrDefault(s => s.ItemId == item.ItemId);
                        if (sourceStock == null)
                            throw new BusinessException($"No stock record found for item {item.ItemId} in the source warehouse.");
                        var destinationStock = destinationWarehouseStocks.FirstOrDefault(s => s.ItemId == item.ItemId);
                        if (destinationStock == null)
                        {
                            destinationStock = new Stock
                            {
                                StockId = Guid.NewGuid(),
                                WarehouseId = transaction.DestinationWarehouseId.Value,
                                ItemId = item.ItemId,
                                Quantity = 0
                            };
                            destinationWarehouseStocks.Add(destinationStock);
                            await _stocksRepository.AddStock(destinationStock);
                            //_context.Stocks.Add(destinationStock);
                        }
                        var quantityChangeSource = sourceStock.Quantity - item.Quantity;
                        if (quantityChangeSource < 0)
                            throw new BusinessException($"Insufficient stock for item {item.ItemId} to transfer to destination warehouse.");

                        sourceStock.Quantity -= item.Quantity;
                        destinationStock.Quantity += item.Quantity;
                    }
                    break;

                case TransactionType.ReturnFromClient:
                    if (transaction.DestinationWarehouseId == null || transaction.ClientId == null)
                        throw new BusinessException("Return transaction requires destination warehouse and client.");
                    var stocksReturned = stocksQuery.Where(s => s.WarehouseId == transaction.DestinationWarehouseId).ToList();
                    //var stocksReturned = await stocksQuery.Where(s => s.WarehouseId == transaction.DestinationWarehouseId).ToListAsync(cancellationToken);
                    foreach(var item in transaction.TransactionItems)
                    {
                        var stock = stocksReturned.FirstOrDefault(s => s.ItemId == item.ItemId);
                        if (stock == null)
                        {
                            stock = new Stock
                            {
                                StockId = Guid.NewGuid(),
                                WarehouseId = transaction.DestinationWarehouseId.Value,
                                ItemId = item.ItemId,
                                Quantity = 0
                            };
                            stocksReturned.Add(stock);
                            await _stocksRepository.AddStock(stock);
                            //_context.Stocks.Add(stock);
                        }
                        stock.Quantity += item.Quantity;
                    }
                    break;
                default:
                    throw new BusinessException("Unsupported transaction type.");
            }

            //change status to approved
            transaction.Status = TransactionStatus.Approved;

            //ave changes
            await _context.SaveChangesAsync(cancellationToken);
            return transaction.TransactionId;
            //return new TransactionDTO
            //{
            //    TransactionId = transaction.TransactionId,
            //    Type = transaction.Type,
            //    Status = transaction.Status,
            //    TransactionDate = transaction.TransactionDate,
            //    SourceWarehouseId = transaction.SourceWarehouseId,
            //    DestinationWarehouseId = transaction.DestinationWarehouseId,
            //    ClientId = transaction.ClientId,
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
        }
    }
}
