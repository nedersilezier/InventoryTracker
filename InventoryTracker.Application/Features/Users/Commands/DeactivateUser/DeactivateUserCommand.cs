using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Users.Commands.DeactivateUser
{
    public class DeactivateUserCommand: IRequest<UserDTO>
    {
        public string UserId { get; private set; } = default!;
        public DeactivateUserCommand(string userId)
        {
            UserId = userId;
        }
    }
}
