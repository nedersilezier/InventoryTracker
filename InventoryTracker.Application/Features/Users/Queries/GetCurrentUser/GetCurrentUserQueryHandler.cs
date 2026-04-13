using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Users.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, CurrentUserDTO>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUsersService _usersService;

        public GetCurrentUserQueryHandler(ICurrentUserService currentUserService, IUsersService usersService)
        {
            _currentUserService = currentUserService;
            _usersService = usersService;
        }
        public async Task<CurrentUserDTO> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                throw new Exception("User is not authenticated.");
            }
            return await _usersService.GetCurrentUserAsync(userId, cancellationToken);
        }
    }
}
