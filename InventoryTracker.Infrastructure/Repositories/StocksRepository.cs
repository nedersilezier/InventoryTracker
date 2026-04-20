using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Infrastructure.Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly AppDbContext _context;
        public StocksRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<Stock>> GetStocksByItemIdsAsync(IEnumerable<Guid> itemIds, CancellationToken cancellationToken)
        {
            return await _context.Stocks.Where(s => itemIds.Contains(s.ItemId)).ToListAsync(cancellationToken);
        }
        public Task AddStock(Stock stock)
        {
            _context.Stocks.Add(stock);
            return Task.CompletedTask;
        }
    }
}
