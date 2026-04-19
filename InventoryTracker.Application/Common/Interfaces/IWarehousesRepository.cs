using InventoryTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface IWarehousesRepository
    {
        Task<Warehouse?> GetWarehouseByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> WarehouseCodeExistsAsync(string code, CancellationToken cancellationToken);
        Task AddWarehouse(Warehouse warehouse);
    }
}
