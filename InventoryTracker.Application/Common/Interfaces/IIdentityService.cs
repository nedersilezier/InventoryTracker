using InventoryTracker.Application.Features.Users.Commands.CreateUser;
using InventoryTracker.Application.Features.Users.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<CreateUserResult> CreateUserAsync(CreateUserCommand command, CancellationToken cancellationToken);
    }
}
