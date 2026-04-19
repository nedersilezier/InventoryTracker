using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;

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
