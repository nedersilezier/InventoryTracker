using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Warehouses.Queries.GetById
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
