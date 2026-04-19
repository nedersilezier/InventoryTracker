using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.WebAdmin.ViewModels.Clients;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class ClientsController : Controller
    {
        public IActionResult Index(int page = 1, string? searchTerm = null)
        {
            var routeValues = new Dictionary<string, string?>
            {
                ["PageSize"] = 10.ToString(),
            };
            return View(new ClientsIndexViewModel
            {
                Clients = new List<ClientListItemViewModel>(),
                SearchTerm = searchTerm,
                TableFooter = new TableFooterViewModel
                {
                    DisplayedCount = 13,
                    TotalCount = 13,
                    EntityName = "clients",
                    Pagination = new PaginationViewModel
                    {
                        CurrentPage = 1,
                        TotalPages = 1,
                        PageSize = 10,
                        Controller = "Clients",
                        Action = "Index",
                        RouteValues = routeValues
                    }
                }
            });
        }
    }
}
