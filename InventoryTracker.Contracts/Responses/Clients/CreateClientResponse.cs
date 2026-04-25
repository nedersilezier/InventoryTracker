using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Clients
{
    public class CreateClientResponse
    {
        public Guid ClientId { get; set; }
        public string ClientName { get; set; } = default!;
        public string ClientCode { get; set; } = default!;
    }
}
