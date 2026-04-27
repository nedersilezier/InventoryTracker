using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommand: IRequest<UserCreatedDTO>
    {
        public string UserId { get; set; } = default!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = default!;
    }
}
