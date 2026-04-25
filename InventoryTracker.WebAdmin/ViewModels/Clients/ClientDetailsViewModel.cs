using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.WebAdmin.ViewModels.Common;

namespace InventoryTracker.WebAdmin.ViewModels.Clients
{
    public class ClientDetailsViewModel
    {
        public Guid ClientId { get; set; }
        public string Name { get; set; } = default!;
        public string ClientCode { get; set; } = default!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public decimal Saldo { get; set; }
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; } = default!;
        public DateTime? UpdatedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public AddressDetailsViewModel Address { get; set; } = default!;
    }
}
