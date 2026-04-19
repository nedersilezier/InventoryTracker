using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Countries.DTOs
{
    public class CreateCountryParameters
    {
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
    }
}
