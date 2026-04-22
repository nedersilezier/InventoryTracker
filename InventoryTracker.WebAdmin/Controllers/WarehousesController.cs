using InventoryTracker.Contracts.Requests.Warehouses;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Items;
using InventoryTracker.WebAdmin.ViewModels.Warehouses;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class WarehousesController : Controller
    {
        private readonly IWarehousesService _warehousesService;
        public WarehousesController(IWarehousesService warehousesService)
        {
            _warehousesService = warehousesService;
        }
        public async Task<IActionResult> Index(GetWarehousesRequest request, CancellationToken cancellationToken)
        {
            var warehousesListViewModel = await _warehousesService.GetAllAsync(request, cancellationToken);
            return View(warehousesListViewModel);
        }
    }
}
