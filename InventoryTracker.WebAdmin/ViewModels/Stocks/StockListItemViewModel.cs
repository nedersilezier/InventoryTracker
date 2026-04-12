namespace InventoryTracker.WebAdmin.ViewModels.Stocks
{
    public class StockListItemViewModel
    {
        public Guid StockId { get; set; }

        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; } = default!;

        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = default!;

        public decimal Quantity { get; set; }
    }
}
