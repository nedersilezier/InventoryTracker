using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Auth.DTOs
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = default!;
        public DateTime ExpiresAtUtc { get; set; }
        public string UserId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
