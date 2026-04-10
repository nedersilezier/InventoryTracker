using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Users.DTOs
{
    public class CurrentUserDTO
    {
        public string UserId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
