using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.WebAdmin.ViewModels.Common;

namespace InventoryTracker.WebAdmin.ViewModels.Warehouses
{
    public class WarehouseDetailsViewModel
    {
        public Guid WarehouseId { get; set; }
        public string Name { get; set; } = default!;
        public string WarehouseCode { get; set; } = default!;
        public int StockCount { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; } = default!;
        public DateTime? UpdatedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public AddressDetailsViewModel Address { get; set; } = default!;
    }
}
