using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Warehouses.DTOs;
using InventoryTracker.Application.Features.Warehouses.Queries.GetWarehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface IWarehousesQueryService
    {
        Task<WarehouseDTO?> GetWarehouseByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<PagedResult<WarehouseDTO>> GetAllWarehousesAsync(GetWarehousesParameters parameters, CancellationToken cancellationToken);
    }
}
