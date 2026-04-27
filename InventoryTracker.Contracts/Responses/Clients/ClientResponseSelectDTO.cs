using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Clients
{
    public class ClientResponseSelectDTO
    {
        public Guid ClientId {  get; set; }
        public string Name { get; set; } = default!;
    }
}
