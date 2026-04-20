using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Stocks.DTOs
{
    public class GetStocksParameters
    {
        public Guid? WarehouseId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; } = 1;
        public string? SearchTerm { get; set; }
    }
}
