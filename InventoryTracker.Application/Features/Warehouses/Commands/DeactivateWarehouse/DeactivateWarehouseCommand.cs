using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Warehouses.Commands.DeactivateWarehouse
{
    public class DeactivateWarehouseCommand: IRequest<WarehouseDTO?>
    {
        public Guid WarehouseId { get; private set; }
        public DeactivateWarehouseCommand(Guid warehouseId)
        {
            WarehouseId = warehouseId;
        }
    }
}
