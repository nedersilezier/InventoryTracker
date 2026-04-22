using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Items.DTOs
{
    public class ItemCreatedDTO
    {
        public Guid ItemId {  get; set; }
        public string Name { get; set; } = default!;
        public string SKU { get; set; } = default!;
    }
}
