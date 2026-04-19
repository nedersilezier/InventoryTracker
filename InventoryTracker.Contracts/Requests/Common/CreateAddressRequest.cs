using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Requests.Common
{
    public class CreateAddressRequest
    {
        public string Street { get; set; } = default!;
        public string HouseNumber { get; set; } = default!;
        public string? ApartmentNumber { get; set; }
        public string PostalCode { get; set; } = default!;
        public string City { get; set; } = default!;
        public Guid CountryId { get; set; }
    }
}
