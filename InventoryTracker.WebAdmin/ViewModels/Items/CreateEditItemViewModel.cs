using System.ComponentModel.DataAnnotations;

namespace InventoryTracker.WebAdmin.ViewModels.Items
{
    public class CreateEditItemViewModel
    {
        public Guid? ItemId { get; set; }

        [Required(ErrorMessage = "Item name is required")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 150 characters")]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "SKU is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "SKU must be between 1 and 50 characters")]
        [RegularExpression(@"^[A-Z0-9\-]+$", ErrorMessage = "SKU must contain only uppercase letters, numbers and '-'")]
        public string SKU { get; set; } = string.Empty;


        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }


        [Required(ErrorMessage = "Unit of measure is required")]
        [StringLength(20)]
        public string UnitOfMeasure { get; set; } = "pcs";


        [Range(0, 99999999.99, ErrorMessage = "Credit value must be between 0 and 99,999,999.99")]
        public decimal CreditValue { get; set; }


        [Range(0, 99999999.99, ErrorMessage = "Weight must be between 0 and 99,999,999.99")]
        public decimal Weight { get; set; }
    }
}
