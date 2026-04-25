using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Countries.DTOs
{
    public class InternalCountrySelectDTO
    {
        public Guid CountryId { get; set; }
        public string CountryName { get; set; } = default!;
    }
}
