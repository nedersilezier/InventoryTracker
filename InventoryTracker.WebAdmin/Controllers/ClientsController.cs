using InventoryTracker.Contracts.Requests.Clients;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.Clients;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IClientsService _clientsService;
        public ClientsController(IClientsService clientsService)
        {
            _clientsService = clientsService;
        }
        public async Task<IActionResult> Index(GetClientsRequest request, CancellationToken cancellationToken)
        {
            var clientsListViewModel = await _clientsService.GetAllAsync(request, cancellationToken);
            return View(clientsListViewModel);
        }
    }
}
