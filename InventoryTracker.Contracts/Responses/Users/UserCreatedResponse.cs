using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Responses.Users
{
    public class UserCreatedResponse
    {
        public string UserId { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
