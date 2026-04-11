using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Requests.Countries
{
    public class CreateCountryRequest
    {
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
    }
}
