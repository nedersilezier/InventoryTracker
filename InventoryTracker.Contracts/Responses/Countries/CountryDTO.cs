using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Countries
{
    public class CountryDTO
    {
        public Guid CountryId { get; set; }
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
    }
}
