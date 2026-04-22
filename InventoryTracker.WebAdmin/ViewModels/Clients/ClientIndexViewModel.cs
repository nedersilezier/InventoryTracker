using InventoryTracker.WebAdmin.ViewModels.HelperVMs;

namespace InventoryTracker.WebAdmin.ViewModels.Clients
{
    public class ClientsIndexViewModel
    {
        public string? SearchTerm { get; set; }
        public int PageSize { get; set; }
        public List<ClientListItemViewModel> Clients { get; set; } = new();
        public TableFooterViewModel TableFooter { get; set; } = new();
    }
}
