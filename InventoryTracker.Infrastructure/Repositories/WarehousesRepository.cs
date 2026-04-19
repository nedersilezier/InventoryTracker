using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Infrastructure.Repositories
{
    public class WarehousesRepository: IWarehousesRepository
    {
        private readonly AppDbContext _context;
        public WarehousesRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Warehouse?> GetWarehouseByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Warehouses
                .Include(w => w.Address)
                .ThenInclude(a => a.Country)
                .FirstOrDefaultAsync(w => w.WarehouseId == id, cancellationToken);
        }
        public async Task<bool> WarehouseCodeExistsAsync(string code, CancellationToken cancellationToken)
        {
            return await _context.Warehouses.AnyAsync(w => w.Code == code, cancellationToken);
        }
        public Task AddWarehouse(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            return Task.CompletedTask;
        }
    }
}
