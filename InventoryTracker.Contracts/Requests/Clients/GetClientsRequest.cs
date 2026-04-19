using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Requests.Clients
{
    public class GetClientsRequest
    {
        public string? SearchTerm { get; init; }
        public int PageNumber { get; init; } = 1;
        public int? PageSize { get; init; }
    }
}
