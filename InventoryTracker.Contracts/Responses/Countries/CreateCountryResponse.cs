using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Countries
{
    public class CreateCountryResponse
    {
        public Guid CountryId { get; set; }
        public string Name { get; set; } = default!;
    }
}
