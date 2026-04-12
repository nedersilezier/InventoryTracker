namespace InventoryTracker.WebAdmin.ViewModels.Countries
{
    public class CountryListItemViewModel
    {
        public Guid CountryId { get; set; }
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
