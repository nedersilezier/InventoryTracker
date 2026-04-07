namespace InventoryTracker.API.Requests.Clients
{
    public class UpdateClientRequest
    {
        public string Name { get; set; } = default!;
        public string ClientCode { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public UpdateClientAddressRequest Address { get; set; } = default!;
        public class UpdateClientAddressRequest
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
