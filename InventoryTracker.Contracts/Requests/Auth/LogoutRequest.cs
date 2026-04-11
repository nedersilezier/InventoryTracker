using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Requests.Auth
{
    public class LogoutRequest
    {
        public string RefreshToken { get; set; } = default!;
    }
}
