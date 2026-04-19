using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Clients.DTOs
{
    public class GetClientsParameters
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; } = 1;
        public string? SearchTerm { get; set; }
    }
}
