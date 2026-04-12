namespace InventoryTracker.WebAdmin.ViewModels.Warehouses
{
    public class WarehouseListItemViewModel
    {
        public Guid WarehouseId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        public string FullAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;

        public int StockEntriesCount { get; set; }
    }
}
