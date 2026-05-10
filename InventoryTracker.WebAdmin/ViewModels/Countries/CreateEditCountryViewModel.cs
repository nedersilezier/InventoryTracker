using System.ComponentModel.DataAnnotations;

namespace InventoryTracker.WebAdmin.ViewModels.Countries
{
    public class CreateEditCountryViewModel
    {
        public Guid? CountryId { get; set; }

        [Required(ErrorMessage = "Country name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "ISO code is required")]
        [StringLength(3, MinimumLength = 2, ErrorMessage = "ISO code must be between 1 and 3 characters")]
        public string CountryCode { get; set; } = default!;
    }
}
