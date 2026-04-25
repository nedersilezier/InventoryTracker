using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Countries
{
    public class CountryResponseSelectDTO
    {
        public Guid CountryId { get; set; }
        public string CountryName { get; set; } = default!;
    }
}
