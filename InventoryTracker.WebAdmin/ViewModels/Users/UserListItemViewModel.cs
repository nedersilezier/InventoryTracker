namespace InventoryTracker.WebAdmin.ViewModels.Users
{
    public class UserListItemViewModel
    {
        public string UserId { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public bool IsActive { get; set; }
        public string UserName { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}
