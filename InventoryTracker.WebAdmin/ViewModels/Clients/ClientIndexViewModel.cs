using InventoryTracker.WebAdmin.ViewModels.HelperVMs;

namespace InventoryTracker.WebAdmin.ViewModels.Clients
{
    public class ClientsIndexViewModel
    {
        public string? SearchTerm { get; set; }
        public List<ClientListItemViewModel> Clients { get; set; } = new();

        public int TotalCount { get; set; }
        public PaginationViewModel Pagination { get; set; } = new();
    }
}
