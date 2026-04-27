using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Users.Commands.ActivateUser;
using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Users.Commands.DeactivateUser
{
    public class DeactivateUserCommandHandler: IRequestHandler<DeactivateUserCommand, UserDTO>
    {
        private readonly IUsersService _usersService;
        public DeactivateUserCommandHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }
        public async Task<UserDTO> Handle(DeactivateUserCommand command, CancellationToken cancellationToken)
        {
            var id = command.UserId;
            return await _usersService.DeactivateUserAsync(id, cancellationToken);
        }
    }
}
