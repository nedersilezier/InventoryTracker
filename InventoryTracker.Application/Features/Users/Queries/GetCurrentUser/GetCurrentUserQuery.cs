using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Users.Queries.GetCurrentUser
{
    public class GetCurrentUserQuery: IRequest<UserDTO>
    {
    }
}
