using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Items.DTOs
{
    public class InternalItemSelectDTO
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = default!;
        public string UnitOfMeasure { get; set; } = default!;
    }
}
