using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedResult<UserDTO>>
    {
        private readonly IUsersService _usersService;
        public GetUsersQueryHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }
        public async Task<PagedResult<UserDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var parameters = new GetUsersParameters
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                SearchTerm = request.SearchTerm
            };
            return await _usersService.GetAllUsersAsync(parameters, cancellationToken);
        }
    }
}
