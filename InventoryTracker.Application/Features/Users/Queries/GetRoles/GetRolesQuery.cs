using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Users.Queries.GetRoles
{
    public class GetRolesQuery: IRequest<IReadOnlyList<RoleDTO>>
    {
    }
}
