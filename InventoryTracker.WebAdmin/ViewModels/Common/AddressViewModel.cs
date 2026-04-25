using System.ComponentModel.DataAnnotations;

namespace InventoryTracker.WebAdmin.ViewModels.Common
{
    public class AddressViewModel
    {
        public Guid? AddressId {  get; set; }

        [Required(ErrorMessage = "Street is required")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Street must be between 3 and 150 characters")]
        public string Street { get; set; } = default!;

        [Required(ErrorMessage = "House number is required")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "House number must be between 1 and 20 characters")]
        public string HouseNumber { get; set; } = default!;

        [StringLength(20, MinimumLength = 1, ErrorMessage = "Apartment number must be between 1 and 20 characters")]
        public string? ApartmentNumber { get; set; }

        [Required(ErrorMessage = "Postal code is required")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Postal code must be between 1 and 20 characters")]
        public string PostalCode { get; set; } = default!;

        [Required(ErrorMessage = "City is required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "City name must be between 2 and 150 characters")]
        public string City { get; set; } = default!;

        [Required(ErrorMessage = "Country is required")]
        public Guid CountryId { get; set; }
    }
}
