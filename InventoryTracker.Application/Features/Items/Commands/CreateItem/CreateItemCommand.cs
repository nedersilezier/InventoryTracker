using InventoryTracker.Application.Features.Items.DTOs.Items;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

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
