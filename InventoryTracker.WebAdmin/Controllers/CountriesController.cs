using InventoryTracker.WebAdmin.Filters;
using InventoryTracker.WebAdmin.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    [RequireAuth]
    public class CountriesController: Controller
    {
        private readonly ICountriesService _countriesService;
        public CountriesController(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var countries = await _countriesService.GetAllAsync(cancellationToken);
            return View(countries);
        }
    }
}
