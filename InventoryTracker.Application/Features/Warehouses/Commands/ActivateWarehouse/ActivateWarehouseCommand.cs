using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Warehouses.Commands.ActivateWarehouse
{
    public class ActivateWarehouseCommand: IRequest<WarehouseDTO?>
    {
        public Guid WarehouseId { get; private set; }
        public ActivateWarehouseCommand(Guid warehouseId)
        {
            WarehouseId = warehouseId;
        }
    }
}
