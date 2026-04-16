using InventoryTracker.WebAdmin.ViewModels.HelperVMs;

namespace InventoryTracker.WebAdmin.ViewModels.Users
{
    public class UsersIndexViewModel
    {
        public List<UserListItemViewModel> Users { get; set; } = new List<UserListItemViewModel>();
        public PaginationViewModel Pagination { get; set; } = new PaginationViewModel();
        public string? SearchTerm { get; set; }
        public int TotalCount { get; set; }
    }
}
