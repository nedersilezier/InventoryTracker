using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Users.Queries.GetById
{
    public class GetUserByIdQueryHandler: IRequestHandler<GetUserByIdQuery, UserDTO?>
    {
        private readonly IUsersService _usersService;
        public GetUserByIdQueryHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }
        public async Task<UserDTO?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var user = await _usersService.GetUserByIdAsync(query.UserId, cancellationToken);
            return user;
        }
    }
}
