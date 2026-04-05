using InventoryTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Items.DTOs.Items
{
    public class ItemDTO
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = default!;
        public string SKU { get; set; } = default!;
        public string? Description { get; set; }
        public string UnitOfMeasure { get; set; } = "pcs";
        public decimal CreditValue { get; set; }
        public decimal Weight { get; set; }
        public bool IsActive { get; set; }

        //test
        public DateTime CreatedAt { get; set; } = default!;
        public string CreatedBy { get; set; } = default!;
        public DateTime? UpdatedAt { get; set; } = default!;
        public string? UpdatedBy { get; set; } = default!;
        public DateTime? DeletedAt { get; set; } = default!;
        public string? DeletedBy { get; set; } = default!;
    }
}
