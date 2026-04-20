using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Stocks
{
    public class StocksResponseDTO
    {
        public Guid StockId { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = default!;
        public string WarehouseName { get; set; } = default!;
        public string SKU { get; set; } = default!;
        public string UnitOfMeasure { get; set; } = default!;
        public decimal CreditValue { get; set; }
        public decimal Weight { get; set; }
        public decimal Quantity { get; set; }
    }
}
