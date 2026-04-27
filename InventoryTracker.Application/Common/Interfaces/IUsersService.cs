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
        Task<UserDTO> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
        Task<PagedResult<UserDTO>> GetAllUsersAsync(GetUsersParameters parameters, CancellationToken cancellationToken);
        Task<UserDTO> ActivateUserAsync(string userId, CancellationToken cancellationToken);
        Task<UserDTO> DeactivateUserAsync(string userId, CancellationToken cancellationToken);
    }
}
