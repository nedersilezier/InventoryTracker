using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Warehouses.DTOs;
using InventoryTracker.Domain.Entities;
using MediatR;

namespace InventoryTracker.Application.Features.Warehouses.Queries.GetWarehouses
{
    public class GetWarehouseByIdQueryHandler : IRequestHandler<GetWarehouseByIdQuery, WarehouseDTO?>
    {
        private readonly IWarehousesQueryService _warehousesQueryService;
        public GetWarehouseByIdQueryHandler(IWarehousesQueryService warehousesQueryService)
        {
            _warehousesQueryService = warehousesQueryService;
        }
        public async Task<WarehouseDTO?> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
        {
            var warehouse = await _warehousesQueryService.GetWarehouseByIdAsync(request.WarehouseId, cancellationToken);
            if (warehouse == null)
            {
                throw new RecordNotFoundException(nameof(Warehouse), request.WarehouseId);
            }
            return warehouse;
        }
    }
}
