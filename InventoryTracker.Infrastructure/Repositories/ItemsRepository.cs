using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Infrastructure.Repositories
{
    public class ItemsRepository: IItemsRepository
    {
        private readonly AppDbContext _context;
        public ItemsRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task AddItem(Item item)
        {
            _context.Items.Add(item);
            return Task.CompletedTask;
        }
        public async Task<Item?> GetItemByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Items.FirstOrDefaultAsync(i => i.ItemId == id, cancellationToken);
        }
        public async Task<IReadOnlyList<Item>> GetActiveItemsByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        {
            return await _context.Items.Where(i => i.IsActive == true && ids.Contains(i.ItemId)).ToListAsync(cancellationToken);
        }
        public async Task<bool> SKUExistsAsync(string sku, CancellationToken cancellationToken, Guid? excludedId = null)
        {
            if (excludedId == null)
                return await _context.Items.AnyAsync(i => i.SKU == sku, cancellationToken);
            else
                return await _context.Items.AnyAsync(i => i.SKU == sku && i.ItemId != excludedId, cancellationToken);
        }
    }
}
