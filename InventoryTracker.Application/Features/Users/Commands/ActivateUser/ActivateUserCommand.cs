using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Users.Commands.ActivateUser
{
    public class ActivateUserCommand: IRequest<UserDTO>
    {
        public string UserId { get; private set; } = default!;
        public ActivateUserCommand(string userId)
        {
            UserId = userId;
        }
    }
}
