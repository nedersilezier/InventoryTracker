using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Clients.DTOs
{
    public class InternalClientSelectDTO
    {
        public Guid ClientId { get; set; }
        public string Name { get; set; } = default!;
    }
}
