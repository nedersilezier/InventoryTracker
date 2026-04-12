using InventoryTracker.WebAdmin.ViewModels.Clients;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;

namespace InventoryTracker.WebAdmin.ViewModels.Countries
{
    public class CountriesIndexViewModel
    {
        public string? SearchTerm { get; set; }
        public int TotalCount { get; set; }
        public List<CountryListItemViewModel> Countries { get; set; } = new();
        public PaginationViewModel Pagination { get; set; } = new();
    }
}
