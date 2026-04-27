using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Clients.DTOs;
using InventoryTracker.Application.Features.Warehouses.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Warehouses.Queries.GetAll
{
    public class GetWarehousesQueryHandler : IRequestHandler<GetWarehousesQuery, PagedResult<WarehouseDTO>>
    {
        private readonly IWarehousesQueryService _warehousesQueryService;
        public GetWarehousesQueryHandler(IWarehousesQueryService warehousesQueryService)
        {
            _warehousesQueryService = warehousesQueryService;
        }
        public async Task<PagedResult<WarehouseDTO>> Handle(GetWarehousesQuery request, CancellationToken cancellationToken)
        {
            request.PageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            var parameters = new GetWarehousesParameters
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                SearchTerm = request.SearchTerm
            };
            return await _warehousesQueryService.GetAllWarehousesAsync(parameters, cancellationToken);
        }
    }
}
