using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Users.DTOs
{
    public class CreateUserResult
    {
        public bool Succeeded { get; init; }
        public string? UserId { get; init; }
        public string? Email { get; init; }
        public IReadOnlyList<string> Errors { get; init; } = new List<string>();
    }
}
