using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Users.DTOs
{
    public class UserCreatedDTO
    {
        public string? UserId { get; set; }
        public string Email { get; set; } = default!;
    }
}
