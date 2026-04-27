using Azure.Core;
using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Users.DTOs;
using InventoryTracker.Application.Features.Users.Queries.GetUsers;
using InventoryTracker.Infrastructure.Identity;
using InventoryTracker.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Infrastructure.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<UserDTO> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new RecordNotFoundException(nameof(ApplicationUser), userId);
            var roles = await _userManager.GetRolesAsync(user);
            return new UserDTO
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Roles = roles
            };
        }
        public async Task<PagedResult<UserDTO>> GetAllUsersAsync(GetUsersParameters parameters, CancellationToken cancellationToken)
        {
            var query = _userManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(u =>
                (u.UserName != null && u.UserName.Contains(parameters.SearchTerm))
                || (u.Email != null && u.Email.Contains(parameters.SearchTerm))
                || (u.FirstName != null && u.FirstName.Contains(parameters.SearchTerm))
                || (u.LastName != null && u.LastName.Contains(parameters.SearchTerm))
                );
            }
            var totalCount = await query.CountAsync(cancellationToken);
            var totalPages = totalCount == 0 ? 1 : (int)Math.Ceiling((double)totalCount / parameters.PageSize);
            var pageNumber = parameters.PageNumber < 1 ? 1 : parameters.PageNumber;
            if(pageNumber > totalPages)
                pageNumber = totalPages;

            var users = await query.OrderBy(u => u.Email).Skip((pageNumber - 1) * parameters.PageSize).Take(parameters.PageSize).ToListAsync(cancellationToken);

            var userDTOs = new List<UserDTO>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDTOs.Add(new UserDTO
                {
                    UserId = user.Id,
                    Email = user.Email ?? string.Empty,
                    FirstName = user.FirstName ?? string.Empty,
                    LastName = user.LastName ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    IsActive = user.IsActive,
                    Roles = roles
                });
            }
            return new PagedResult<UserDTO>
            {
                Items = userDTOs,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = parameters.PageSize,
                TotalCount = totalCount
            };
        }
        public async Task<UserDTO> ActivateUserAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new RecordNotFoundException(nameof(ApplicationUser), userId);
            user.IsActive = true;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new BusinessException("Failed to update the user.");
            return new UserDTO
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty
            };
        }
        public async Task<UserDTO> DeactivateUserAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new RecordNotFoundException(nameof(ApplicationUser), userId);
            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new BusinessException("Failed to update the user.");
            return new UserDTO
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty
            };
        }
    }
}
