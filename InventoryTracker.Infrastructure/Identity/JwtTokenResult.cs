using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Infrastructure.Identity
{
    public class JwtTokenResult
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
    }
}
