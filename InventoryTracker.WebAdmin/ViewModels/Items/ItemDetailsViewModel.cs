using System.ComponentModel.DataAnnotations;

namespace InventoryTracker.WebAdmin.ViewModels.Items
{
    public class ItemDetailsViewModel
    {
        public Guid? ItemId { get; set; }
        public string Name { get; set; } = default!;
        public string SKU { get; set; } = default!;
        public string? Description { get; set; }
        public string UnitOfMeasure { get; set; } = default!;
        public decimal CreditValue { get; set; }
        public string CreditValueDisplay
        {
            get
            {
                return CreditValue.ToString("C");
            }
        }
        public decimal Weight { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveDisplay
        {
            get
            {
                return IsActive ? "Yes" : "No";
            }
        }
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; } = default!;
        public DateTime? UpdatedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
