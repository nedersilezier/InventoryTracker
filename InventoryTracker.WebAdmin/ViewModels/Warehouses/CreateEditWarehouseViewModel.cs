using InventoryTracker.WebAdmin.ViewModels.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace InventoryTracker.WebAdmin.ViewModels.Warehouses
{
    public class CreateEditWarehouseViewModel
    {
        [Key]
        public Guid? WarehouseId { get; set; }

        [Required(ErrorMessage = "Warehouse name is required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Name must be between 3 and 200 characters")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Warehouse code is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Warehouse code must be between 1 and 50 characters")]
        public string WarehouseCode { get; set; } = default!;

        [Required(ErrorMessage = "Address is required")]
        public AddressViewModel Address { get; set; } = new();
        public List<SelectListItem> AvailableCountries { get; set; } = new();
    }
}
