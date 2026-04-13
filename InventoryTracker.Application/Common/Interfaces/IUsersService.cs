using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Users.DTOs;
using InventoryTracker.Application.Features.Users.Queries.GetUsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface IUsersService
    {
        Task<CurrentUserDTO> GetCurrentUserAsync(string userId, CancellationToken cancellationToken);
        Task<PagedResult<UserDTO>> GetAllUsersAsync(GetUsersParameters parameters, CancellationToken cancellationToken);
    }
}
