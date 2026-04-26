namespace InventoryTracker.WebAdmin.ViewModels.Warehouses
{
    public class WarehouseListItemViewModel
    {
        public Guid WarehouseId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        public string AddressLine1 { get; set; } = default!;
        public string AddressLine2 { get; set; } = default!;
        public string City { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;

        public int StockEntriesCount { get; set; }
        public bool IsActive { get; set; }
    }
}
