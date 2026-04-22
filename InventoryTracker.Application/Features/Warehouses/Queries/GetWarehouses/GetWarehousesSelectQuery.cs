using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Warehouses.Queries.GetWarehouses
{
    public class GetWarehousesSelectQuery: IRequest<IReadOnlyList<InternalWarehouseSelectDTO>>
    {
    }
}
