using InventoryTracker.Application.Features.Items.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Items.Commands.UpdateItem
{
    public class UpdateItemCommand: IRequest<ItemDTO?>
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = default!;
        public string SKU { get; set; } = default!;
        public string? Description { get; set; }
        public string UnitOfMeasure { get; set; } = default!;
        public decimal CreditValue { get; set; }
        public decimal Weight { get; set; }
    }
}
