using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Requests.Countries
{
    public class GetCountriesRequest
    {
        public string? SearchTerm { get; init; }
        public int PageNumber { get; init; } = 1;
        public int? PageSize { get; init; }
    }
}
