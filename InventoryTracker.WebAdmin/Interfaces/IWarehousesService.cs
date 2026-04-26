using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Warehouses;
using InventoryTracker.Contracts.Responses.Warehouses;
using InventoryTracker.WebAdmin.ViewModels.Warehouses;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface IWarehousesService
    {
        Task<ServiceResult<WarehousesIndexViewModel>> GetAllAsync(GetWarehousesRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<CreateEditWarehouseViewModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ServiceResult<WarehouseDetailsViewModel>> GetDetailsByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ServiceResult<CreateWarehouseResponse>> CreateWarehouseAsync(CreateWarehouseRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<CreateWarehouseResponse>> UpdateWarehouseAsync(Guid id, UpdateWarehouseRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<CreateWarehouseResponse>> DeactivateWarehouseAsync(Guid id, CancellationToken cancellationToken);
        Task<ServiceResult<CreateWarehouseResponse>> ActivateWarehouseAsync(Guid id, CancellationToken cancellationToken);
    }
}
