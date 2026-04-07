namespace InventoryTracker.API.Requests.Items
{
    public class UpdateItemRequest
    {
        public string Name { get; set; } = default!;
        public string SKU { get; set; } = default!;
        public string? Description { get; set; }
        public string UnitOfMeasure { get; set; } = default!;
        public decimal CreditValue { get; set; }
        public decimal Weight { get; set; }
    }
}
