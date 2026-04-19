using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Requests.Warehouses
{
    public class GetWarehousesRequest
    {
        public string? SearchTerm { get; init; }
        public int PageNumber { get; init; } = 1;
        public int? PageSize { get; init; }
    }
}
