using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Users.Queries.GetById
{
    public class GetUserByIdQuery: IRequest<UserDTO?>
    {
        public string UserId { get; private set; } = default!;
        public GetUserByIdQuery(string userId)
        {
            UserId = userId;
        }
    }
}
