using InventoryTracker.Contracts.Requests.Users;
using InventoryTracker.WebAdmin.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;
        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        public async Task<IActionResult> Index(GetUsersRequest request, CancellationToken cancellationToken)
        {
            var usersIndexViewModel = await _usersService.GetAllAsync(request, cancellationToken);
            return View(usersIndexViewModel);
        }
    }
}
