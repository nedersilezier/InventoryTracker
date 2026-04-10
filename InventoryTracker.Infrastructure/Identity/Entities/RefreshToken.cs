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
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public bool IsRevoked
        { 
            get { return RevokedAt.HasValue; }
        }
        public bool IsExpired
        {
            get { return DateTime.UtcNow >= ExpiresAt; }
        }

    }
}
