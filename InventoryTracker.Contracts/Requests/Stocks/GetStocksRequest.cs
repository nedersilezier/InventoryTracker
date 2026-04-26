using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Requests.Stocks
{
    public class GetStocksRequest
    {
        public Guid? SelectedWarehouseId { get; set;  } = default(Guid?);
        public int? PageSize { get; set; }
        public int PageNumber { get; set; } = 1;
        public string? SearchTerm { get; set; }
    }
}
