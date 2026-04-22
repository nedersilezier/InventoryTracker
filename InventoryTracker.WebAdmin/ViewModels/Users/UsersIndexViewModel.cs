using InventoryTracker.WebAdmin.ViewModels.HelperVMs;

namespace InventoryTracker.WebAdmin.ViewModels.Users
{
    public class UsersIndexViewModel
    {
        public List<UserListItemViewModel> Users { get; set; } = new List<UserListItemViewModel>();
        public TableFooterViewModel TableFooter { get; set; } = new TableFooterViewModel();
        public string? SearchTerm { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
    }
}
