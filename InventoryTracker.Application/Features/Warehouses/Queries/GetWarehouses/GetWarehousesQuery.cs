using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Warehouses.Queries.GetWarehouses
{
    public class GetWarehousesQuery: IRequest<PagedResult<WarehouseDTO>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; } = 1;
        public string? SearchTerm { get; set; }
    }
}
