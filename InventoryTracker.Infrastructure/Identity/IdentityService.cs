using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Users.Commands.CreateUser;
using InventoryTracker.Application.Features.Users.DTOs;
using InventoryTracker.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InventoryTracker.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<CreateUserResult> CreateUserAsync(CreateUserCommand command, CancellationToken cancellationToken)
        {
            //ensure email and username are unique and role exists before creating the user
            var emailExists = await _userManager.FindByEmailAsync(command.Email);
            if (emailExists != null)
            {
                return new CreateUserResult
                {
                    Succeeded = false,
                    Errors = [$"User with email '{command.Email}' already exists."]
                };
            }
            var usernameExists = await _userManager.FindByNameAsync(command.Email);
            if (usernameExists != null)
            {
                return new CreateUserResult
                {
                    Succeeded = false,
                    Errors = [$"User with user name '{command.Email}' already exists."]
                };
            }
            var roleExists = await _roleManager.RoleExistsAsync(command.Role);
            if (!roleExists)
            {
                return new CreateUserResult
                {
                    Succeeded = false,
                    Errors = [$"Role '{command.Role}' does not exist."]
                };
            }

            //create the user object
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = command.Email,
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                PhoneNumber = command.PhoneNumber,
                IsActive = true,
                EmailConfirmed = true
            };

            //create the user in the database and add to role
            var result = await _userManager.CreateAsync(user, command.Password);
            if (!result.Succeeded)
            {
                return new CreateUserResult
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, command.Role);
            if (!addToRoleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                return new CreateUserResult
                {
                    Succeeded = false,
                    Errors = addToRoleResult.Errors.Select(e => e.Description).ToList()
                };
            }

            await _userManager.AddToRoleAsync(user, command.Role);

            return new CreateUserResult
            {
                Succeeded = true,
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Role = command.Role,
                IsActive = user.IsActive
            };
        }
    }
}