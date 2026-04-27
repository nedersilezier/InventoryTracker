using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace InventoryTracker.WebAdmin.ViewModels.Users
{
    public class CreateUserViewModel
    {

        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        public string? FirstName { get; set; }

        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [StringLength(256, MinimumLength = 5, ErrorMessage = "Email must be between 5 and 256 characters")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 256 characters")]
        public string Password { get; set; } = default!;

        [StringLength(50, ErrorMessage = "Phone number cannot exceed 50 characters")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string SelectedRole { get; set; } = default!;
        public List<SelectListItem> AvailableRoles { get; set; } = new();
    }
}
