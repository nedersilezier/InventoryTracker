using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebOperator.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
