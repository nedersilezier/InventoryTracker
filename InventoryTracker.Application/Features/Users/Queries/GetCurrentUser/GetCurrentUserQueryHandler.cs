using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Users.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDTO>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUsersService _usersService;

        public GetCurrentUserQueryHandler(ICurrentUserService currentUserService, IUsersService usersService)
        {
            _currentUserService = currentUserService;
            _usersService = usersService;
        }
        public async Task<UserDTO> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                throw new Exception("User is not authenticated.");
            }
            return await _usersService.GetUserByIdAsync(userId, cancellationToken);
        }
    }
}
