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
        public string? UserName { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? PhoneNumber { get; init; }
        public string? Role { get; init; }
        public bool IsActive { get; init; }
        public IReadOnlyList<string> Errors { get; init; } = new List<string>();
    }
}
