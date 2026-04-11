using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Infrastructure.Identity.Entities
{
    public class RefreshToken
    {
        public Guid RefreshTokenId { get; set; }
        public string Token { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;
        public DateTime CreatedAtUtc { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
        public DateTime? RevokedAtUtc { get; set; }
        public bool IsRevoked
        { 
            get { return RevokedAtUtc.HasValue; }
        }
        public bool IsExpired
        {
            get { return DateTime.UtcNow >= ExpiresAtUtc; }
        }

    }
}
