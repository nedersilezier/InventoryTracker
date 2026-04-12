using InventoryTracker.WebAdmin.ViewModels.Clients;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class ClientsController : Controller
    {
        public IActionResult Index(int page = 1, string? searchTerm = null)
        {
            return View(new ClientsIndexViewModel
            {
                Clients = new List<ClientListItemViewModel>(),
                SearchTerm = searchTerm,
                Pagination = new PaginationViewModel
                {
                    CurrentPage = page,
                    Controller = "Clients",
                    TotalPages = 10,
                    RouteValues = new Dictionary<string, string?> { { "searchTerm", searchTerm } }
                }
            });
        }
    }
}
