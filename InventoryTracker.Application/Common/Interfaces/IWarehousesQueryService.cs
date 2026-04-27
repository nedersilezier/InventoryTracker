using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Warehouses.DTOs;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface IWarehousesQueryService
    {
        Task<PagedResult<WarehouseDTO>> GetAllWarehousesAsync(GetWarehousesParameters parameters, CancellationToken cancellationToken);
        Task<WarehouseDTO?> GetWarehouseByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<WarehouseDetailsDTO?> GetWarehouseDetailsByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<InternalWarehouseSelectDTO>> GetAllWarehousesLookupAsync(CancellationToken cancellationToken);
    }
}
