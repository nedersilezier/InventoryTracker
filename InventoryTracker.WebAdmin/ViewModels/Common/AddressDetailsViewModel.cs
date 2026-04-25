using System.ComponentModel.DataAnnotations;

namespace InventoryTracker.WebAdmin.ViewModels.Common
{
    public class AddressDetailsViewModel
    {
        public string Street { get; set; } = default!;
        public string HouseNumber { get; set; } = default!;
        public string? ApartmentNumber { get; set; }
        public string PostalCode { get; set; } = default!;
        public string City { get; set; } = default!;
        public string CountryName { get; set; } = default!;
    }
}
