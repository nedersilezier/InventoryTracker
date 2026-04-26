using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Countries.DTOs
{
    public class CountryCreatedDTO
    {
        public Guid CountryId {  get; set; }
        public string Name { get; set; } = default!;
    }
}
