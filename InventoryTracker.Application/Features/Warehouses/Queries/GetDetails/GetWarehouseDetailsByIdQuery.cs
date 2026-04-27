using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Warehouses.Queries.GetDetails
{
    public class GetWarehouseDetailsByIdQuery: IRequest<WarehouseDetailsDTO?>
    {
        public Guid WarehouseId { get; private set; }
        public GetWarehouseDetailsByIdQuery(Guid warehouseId)
        {
            WarehouseId = warehouseId;
        }
    }
}
