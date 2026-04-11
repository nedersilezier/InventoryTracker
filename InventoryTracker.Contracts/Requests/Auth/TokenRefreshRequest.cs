using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Requests.Auth
{
    public class TokenRefreshRequest
    {
        public string? RefreshToken {  get; set; }
    }
}
