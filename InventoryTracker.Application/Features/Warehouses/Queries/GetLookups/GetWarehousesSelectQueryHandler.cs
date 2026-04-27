using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Warehouses.Queries.GetLookups
{
    public class GetWarehousesSelectQueryHandler : IRequestHandler<GetWarehousesSelectQuery, IReadOnlyList<InternalWarehouseSelectDTO>>
    {
        private readonly IWarehousesQueryService _warehousesQueryService;
        public GetWarehousesSelectQueryHandler(IWarehousesQueryService warehousesQueryService)
        {
            _warehousesQueryService = warehousesQueryService;
        }
        public async Task<IReadOnlyList<InternalWarehouseSelectDTO>> Handle(GetWarehousesSelectQuery request, CancellationToken cancellationToken)
        {
            return await _warehousesQueryService.GetAllWarehousesLookupAsync(cancellationToken);
        }
    }
}
