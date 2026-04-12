namespace InventoryTracker.WebAdmin.ViewModels.Clients
{
    public class ClientListItemViewModel
    {
        public Guid ClientId { get; set; }
        public string Name { get; set; } = default!;
        public string ClientCode { get; set; } = default!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string FullAddress { get; set; } = default!;
        public string CountryName { get; set; } = default!;

        public decimal Saldo { get; set; } = default!;
    }
}
