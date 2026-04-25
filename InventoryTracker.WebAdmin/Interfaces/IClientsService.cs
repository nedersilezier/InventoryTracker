using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Clients;
using InventoryTracker.Contracts.Responses.Clients;
using InventoryTracker.WebAdmin.ViewModels.Clients;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface IClientsService
    {
        Task<ServiceResult<ClientsIndexViewModel>> GetAllAsync(GetClientsRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<CreateEditClientViewModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ServiceResult<ClientDetailsViewModel>> GetDetailsByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ServiceResult<CreateClientResponse>> CreateClientAsync(CreateClientRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<CreateClientResponse>> UpdateClientAsync(Guid id, UpdateClientRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<CreateClientResponse>> DeactivateClientAsync(Guid id, CancellationToken cancellationToken);
        Task<ServiceResult<CreateClientResponse>> ActivateClientAsync(Guid id, CancellationToken cancellationToken);
    }
}
