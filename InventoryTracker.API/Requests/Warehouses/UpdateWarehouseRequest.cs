namespace InventoryTracker.API.Requests.Warehouses
{
    public class UpdateWarehouseRequest
    {
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
        public UpdateWarehouseAddressRequest Address { get; set; } = default!;
        public class UpdateWarehouseAddressRequest
        {
            public string Street { get; set; } = default!;
            public string HouseNumber { get; set; } = default!;
            public string? ApartmentNumber { get; set; }
            public string PostalCode { get; set; } = default!;
            public string City { get; set; } = default!;
            public Guid CountryId { get; set; }
        }
    }
}
