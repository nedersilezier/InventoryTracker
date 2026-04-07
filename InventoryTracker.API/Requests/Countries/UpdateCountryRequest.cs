namespace InventoryTracker.API.Requests.Countries
{
    public class UpdateCountryRequest
    {
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
    }
}
