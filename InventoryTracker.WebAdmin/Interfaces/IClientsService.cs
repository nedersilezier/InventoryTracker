using InventoryTracker.Contracts.Requests.Clients;
using InventoryTracker.WebAdmin.ViewModels.Clients;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface IClientsService
    {
        Task<ClientsIndexViewModel> GetAllAsync(GetClientsRequest request, CancellationToken cancellationToken);
    }
}
