using InventoryTracker.Contracts.Requests.Countries;
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
        public async Task<IActionResult> Index(GetCountriesRequest request, CancellationToken cancellationToken)
        {
            var countriesListViewModel = await _countriesService.GetAllAsync(request, cancellationToken);
            return View(countriesListViewModel);
        }
    }
}
