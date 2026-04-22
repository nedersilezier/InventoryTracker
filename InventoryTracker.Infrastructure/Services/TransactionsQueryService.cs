using Azure.Core;
using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Transactions.DTOs;
using InventoryTracker.Application.Features.Transactions.Queries.GetTransactions;
using InventoryTracker.Infrastructure.Persistence;
using InventoryTracker.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Infrastructure.Services
{
    public class TransactionsQueryService: ITransactionsQueryService
    {
        private readonly AppDbContext _context;
        public TransactionsQueryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<TransactionListDTO>> GetAllTransactionsAsync(GetTransactionsParameters parameters, CancellationToken cancellationToken)
        {
            var query = _context.Transactions
                .Include(t => t.Client)
                .AsQueryable();
            var selectedTypes = new List<TransactionType>();
            if (parameters.IncludeReturns)
                selectedTypes.Add(TransactionType.ReturnFromClient);
            if (parameters.IncludeIssues)
                selectedTypes.Add(TransactionType.IssueToClient);
            if (parameters.IncludeTransfers)
                selectedTypes.Add(TransactionType.TransferBetweenWarehouses);
            if (parameters.IncludeAdjustments)
                selectedTypes.Add(TransactionType.Adjustment);

            if(selectedTypes.Any())
                query = query.Where(t => selectedTypes.Contains(t.Type));
            else
                query = query.Where(t => false);
            if(parameters.DateFrom.HasValue)
                query = query.Where(t => t.TransactionDate >= parameters.DateFrom.Value);
            if(parameters.DateTo.HasValue)
                query = query.Where(t => t.TransactionDate < parameters.DateTo.Value);

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                query = query.Where(t => (t.Client != null && t.Client.Name.Contains(parameters.SearchTerm))
                                    || (t.SourceWarehouseNameSnapshot != null && t.SourceWarehouseNameSnapshot.Contains(parameters.SearchTerm))
                                    || (t.DestinationWarehouseNameSnapshot != null && t.DestinationWarehouseNameSnapshot.Contains(parameters.SearchTerm))
                                    || (t.ReferenceNumber != null && t.ReferenceNumber.Contains(parameters.SearchTerm)));
            }
            var totalCount = await query.CountAsync(cancellationToken);
            var totalPages = totalCount == 0 ? 1 : (int)Math.Ceiling((double)totalCount / parameters.PageSize);
            var pageNumber = parameters.PageNumber;
            if (pageNumber > totalPages)
                pageNumber = totalPages;

            var transactions = await query.OrderByDescending(t => t.TransactionDate).Skip((pageNumber - 1) * parameters.PageSize).Take(parameters.PageSize).ToListAsync(cancellationToken);
            var transactionsDTO = new List<TransactionListDTO>();
            foreach (var transaction in transactions)
            {
                transactionsDTO.Add(new TransactionListDTO
                {
                    TransactionId = transaction.TransactionId,
                    Type = transaction.Type,
                    Status = transaction.Status,
                    ClientId = transaction.ClientId,
                    ClientName = transaction.Client != null ? transaction.Client.Name : string.Empty,
                    SourceWarehouseId = transaction.SourceWarehouseId,
                    SourceWarehouseNameSnapshot = transaction.SourceWarehouseNameSnapshot,
                    DestinationWarehouseId = transaction.DestinationWarehouseId,
                    DestinationWarehouseNameSnapshot = transaction.DestinationWarehouseNameSnapshot,
                    TransactionDate = transaction.TransactionDate,
                    ReferenceNumber = transaction.ReferenceNumber,
                    FromDisplay = transaction.Type == TransactionType.Adjustment
                        ? null
                        : transaction.Type == TransactionType.ReturnFromClient
                        ? transaction.Client == null ? null : transaction.Client.Name
                        : transaction.Type == TransactionType.TransferBetweenWarehouses
                        ? transaction.SourceWarehouseNameSnapshot
                        : transaction.Type == TransactionType.IssueToClient
                        ? transaction.SourceWarehouseNameSnapshot
                        : null,
                    ToDisplay = transaction.Type == TransactionType.Adjustment
                        ? transaction.SourceWarehouseNameSnapshot
                        : transaction.Type == TransactionType.ReturnFromClient
                        ? transaction.DestinationWarehouseNameSnapshot
                        : transaction.Type == TransactionType.TransferBetweenWarehouses
                        ? transaction.DestinationWarehouseNameSnapshot
                        : transaction.Type == TransactionType.IssueToClient
                        ? transaction.Client == null ? null : transaction.Client.Name
                        : null
                });
            }
            return new PagedResult<TransactionListDTO>
            {
                Items = transactionsDTO,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = parameters.PageSize,
                TotalCount = totalCount
            };
        }
        public async Task<IReadOnlyList<TransactionListDTO>> GetRecentTransactionsAsync(int count, CancellationToken cancellationToken)
        {
            var recentTransactions = await _context.Transactions
                .AsNoTracking()
                .Include(t => t.Client)
                .OrderByDescending(t => t.TransactionDate)
                .Take(count).ToListAsync(cancellationToken);
            if(recentTransactions == null || recentTransactions.Count == 0)
                return new List<TransactionListDTO>();
            var transactionsDTO = new List<TransactionListDTO>();
            transactionsDTO.AddRange(recentTransactions.Select(transaction => new TransactionListDTO
            {
                TransactionId = transaction.TransactionId,
                Type = transaction.Type,
                Status = transaction.Status,
                ClientId = transaction.ClientId,
                ClientName = transaction.Client != null ? transaction.Client.Name : string.Empty,
                SourceWarehouseId = transaction.SourceWarehouseId,
                SourceWarehouseNameSnapshot = transaction.SourceWarehouseNameSnapshot,
                DestinationWarehouseId = transaction.DestinationWarehouseId,
                DestinationWarehouseNameSnapshot = transaction.DestinationWarehouseNameSnapshot,
                TransactionDate = transaction.TransactionDate,
                ReferenceNumber = transaction.ReferenceNumber,
                FromDisplay = transaction.Type == TransactionType.Adjustment
                    ? null
                    : transaction.Type == TransactionType.ReturnFromClient
                    ? transaction.Client == null ? null : transaction.Client.Name
                    : transaction.Type == TransactionType.TransferBetweenWarehouses
                    ? transaction.SourceWarehouseNameSnapshot
                    : transaction.Type == TransactionType.IssueToClient
                    ? transaction.SourceWarehouseNameSnapshot
                    : null,
                ToDisplay = transaction.Type == TransactionType.Adjustment
                    ? transaction.SourceWarehouseNameSnapshot
                    : transaction.Type == TransactionType.ReturnFromClient
                    ? transaction.DestinationWarehouseNameSnapshot
                    : transaction.Type == TransactionType.TransferBetweenWarehouses
                    ? transaction.DestinationWarehouseNameSnapshot
                    : transaction.Type == TransactionType.IssueToClient
                    ? transaction.Client == null ? null : transaction.Client.Name
                    : null
            }));
            return transactionsDTO;
        }
        public async Task<TransactionDTO?> GetTransactionByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions
            .AsNoTracking()
            .Include(t => t.TransactionItems)
            .Include(t => t.Client)
            .Where(t => t.TransactionId == id)
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
            return transaction;
        }
    }
}
