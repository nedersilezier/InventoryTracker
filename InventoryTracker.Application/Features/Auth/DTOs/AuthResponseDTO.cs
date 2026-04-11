using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Auth.DTOs
{
    public class AuthResponseDTO
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public DateTime AccessTokenExpiresAtUtc { get; set; }
        public DateTime RefreshTokenExpiresAtUtc { get; set; }
        public string UserId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
