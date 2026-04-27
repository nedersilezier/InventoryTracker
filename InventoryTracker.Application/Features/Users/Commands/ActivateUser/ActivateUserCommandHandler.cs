using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;


namespace InventoryTracker.Application.Features.Users.Commands.ActivateUser
{
    public class ActivateUserCommandHandler: IRequestHandler<ActivateUserCommand, UserDTO>
    {
        private readonly IUsersService _usersService;
        public ActivateUserCommandHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }
        public async Task<UserDTO> Handle(ActivateUserCommand command, CancellationToken cancellationToken)
        {
            var id = command.UserId;
            return await _usersService.ActivateUserAsync(id, cancellationToken);
        }
    }
}
