using InventoryTracker.Contracts.Requests.Common;
using InventoryTracker.Contracts.Responses.Countries;
using InventoryTracker.WebAdmin.ViewModels.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace InventoryTracker.WebAdmin.ViewModels.Clients
{
    public class CreateEditClientViewModel
    {
        public Guid? ClientId { get; set; }

        [Required(ErrorMessage = "Client name is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 200 characters")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Client code is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Client code must be between 1 and 50 characters")]
        public string ClientCode { get; set; } = default!;

        [StringLength(150, MinimumLength = 5, ErrorMessage = "Email must be between 5 and 150 characters")]
        public string? Email { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Phone number must be between 3 and 50 characters")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public AddressViewModel Address { get; set; } = new();
        public List<SelectListItem> AvailableCountries { get; set; } = new();
    }
}
