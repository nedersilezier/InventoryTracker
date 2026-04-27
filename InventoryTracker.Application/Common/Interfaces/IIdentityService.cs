using InventoryTracker.Application.Features.Auth.DTOs;
using InventoryTracker.Application.Features.Users.Commands.CreateUser;
using InventoryTracker.Application.Features.Users.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<CreateUserResult> CreateUserAsync(string? firstName, string? lastName, string email, string password, string? phoneNumber, string role);
        Task<AuthResponseDTO> LoginAsync(string email, string password, CancellationToken cancellationToken);
        Task LogoutAsync(string refreshToken, CancellationToken cancellationToken);
        Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
        Task<IReadOnlyList<RoleDTO>> GetRolesAsync(CancellationToken cancellationToken);
        Task<CreateUserResult> UpdateUserAsync(string userId, string? firstName, string? lastName, string? phoneNumber, string role);
    }
}
