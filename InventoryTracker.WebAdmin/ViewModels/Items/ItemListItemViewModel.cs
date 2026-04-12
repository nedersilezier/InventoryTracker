namespace InventoryTracker.WebAdmin.ViewModels.Items
{
    public class ItemListItemViewModel
    {
        public Guid ItemId { get; set; }

        public string Name { get; set; } = default!;
        public string SKU { get; set; } = default!;
        public string UnitOfMeasure { get; set; } = default!;
        public decimal CreditValue { get; set; }
        public decimal Weight { get; set; }
        public string Description { get; set; } = default!;
    }
}
