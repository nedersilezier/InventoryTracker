using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Users.Queries.GetRoles
{
    public class GetRolesQueryHandler: IRequestHandler<GetRolesQuery, IReadOnlyList<RoleDTO>>
    {
        private readonly IIdentityService _identityService;
        public GetRolesQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<IReadOnlyList<RoleDTO>> Handle(GetRolesQuery query, CancellationToken cancellationToken)
        {
            return await _identityService.GetRolesAsync(cancellationToken);
        }
    }
}
