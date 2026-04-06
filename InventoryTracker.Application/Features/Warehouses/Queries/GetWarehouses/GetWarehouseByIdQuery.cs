using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Warehouses.Queries.GetWarehouses
{
    public class GetWarehouseByIdQuery: IRequest<WarehouseDTO?>
    {
        public Guid WarehouseId { get; private set; }
        public GetWarehouseByIdQuery(Guid warehouseId)
        {
            WarehouseId = warehouseId;
        }
    }
}
