using InventoryTracker.WebAdmin.ViewModels.Clients;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;

namespace InventoryTracker.WebAdmin.ViewModels.Countries
{
    public class CountriesIndexViewModel
    {
        public string? SearchTerm { get; set; }
        public int PageSize { get; set; }
        public List<CountryListItemViewModel> Countries { get; set; } = new();
        public TableFooterViewModel TableFooter { get; set; } = new();
    }
}
