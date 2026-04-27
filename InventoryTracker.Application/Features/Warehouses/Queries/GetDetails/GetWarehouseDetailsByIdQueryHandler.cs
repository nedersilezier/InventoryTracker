using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Warehouses.DTOs;
using InventoryTracker.Domain.Entities;
using MediatR;

namespace InventoryTracker.Application.Features.Warehouses.Queries.GetDetails
{
    public class GetWarehouseDetailsByIdQueryHandler:IRequestHandler<GetWarehouseDetailsByIdQuery, WarehouseDetailsDTO?>
    {
        private readonly IWarehousesQueryService _warehousesQueryService;
        public GetWarehouseDetailsByIdQueryHandler(IWarehousesQueryService warehousesQueryService)
        {
            _warehousesQueryService = warehousesQueryService;
        }
        public async Task<WarehouseDetailsDTO?> Handle(GetWarehouseDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var warehouse = await _warehousesQueryService.GetWarehouseDetailsByIdAsync(request.WarehouseId, cancellationToken);
            if (warehouse == null)
                throw new RecordNotFoundException(nameof(Warehouse), request.WarehouseId);
            return warehouse;
        }
    }
}
