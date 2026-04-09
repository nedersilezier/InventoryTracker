using InventoryTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Stocks.DTOs
{
    public class StockDetailsDTO
    {
        public Guid StockId { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = default!;
        public string SKU { get; set; } = default!;
        public string UnitOfMeasure { get; set; } = default!;
        public decimal CreditValue { get; set; }
        public decimal Weight { get; set; }
        public decimal Quantity { get; set; }
        public string? CreatedBy { get; set; } = default!;
        public DateTime? CreatedAt { get; set; } = default!;
        public string? UpdatedBy { get; set; } = default!;
        public DateTime? UpdatedAt { get; set; } = default!;
    }
}
