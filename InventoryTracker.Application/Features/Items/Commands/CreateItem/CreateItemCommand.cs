using InventoryTracker.Application.Features.Items.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Items.Commands.CreateItem
{
    public class CreateItemCommand: IRequest<ItemDTO>
    {
        public string Name { get; set; } = default!;
        public string SKU { get; set; } = default!;
        public string? Description { get; set; }
        public string UnitOfMeasure { get; set; } = "pcs";
        public decimal CreditValue { get; set; }
        public decimal Weight { get; set; }
    }
}
