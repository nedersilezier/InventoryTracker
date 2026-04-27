using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace InventoryTracker.WebAdmin.ViewModels.Users
{
    public class EditUserViewModel
    {
        [Required]
        public string UserId { get; set; } = default!;

        [StringLength(256, ErrorMessage = "First name can be maximal 256 characters long")]
        public string? FirstName { get; set; }

        [StringLength(256, ErrorMessage = "Last name can be maximal 256 characters long")]
        public string? LastName { get; set; }

        [StringLength(50, ErrorMessage = "Phone number can be maximal 50 characters long")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string SelectedRole { get; set; } = default!;
        public List<SelectListItem> AvailableRoles { get; set; } = new();
    }
}
