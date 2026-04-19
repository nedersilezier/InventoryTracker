using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Requests.Items
{
    public class GetItemsRequest
    {
        public int PageNumber { get; set; } = 1;
        public int? PageSize { get; set; }
        public string? SearchTerm { get; set; }
    }
}
