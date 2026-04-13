using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQuery: IRequest<PagedResult<UserDTO>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string? SearchTerm { get; set; }
    }
}
