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
        Task<AuthResponseDto> LoginAsync(string? email, string? password);
    }
}
