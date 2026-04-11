using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Auth.Commands.Login;
using InventoryTracker.Application.Features.Auth.DTOs;
using InventoryTracker.Application.Features.Users.Commands.CreateUser;
using InventoryTracker.Application.Features.Users.DTOs;
using InventoryTracker.Infrastructure.Identity.Entities;
using InventoryTracker.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly AppDbContext _context;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;

        public IdentityService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            JwtTokenGenerator jwtTokenGenerator,
            AppDbContext context,
            RefreshTokenGenerator refreshTokenGenerator
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _context = context;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task<CreateUserResult> CreateUserAsync(string? firstName, string? lastName, string email, string password, string? phoneNumber, string role)
        {
            //ensure email and username are unique and role exists before creating the user
            var emailExists = await _userManager.FindByEmailAsync(email);
            if (emailExists != null)
            {
                return new CreateUserResult
                {
                    Succeeded = false,
                    Errors = [$"User with email '{email}' already exists."]
                };
            }
            var usernameExists = await _userManager.FindByNameAsync(email);
            if (usernameExists != null)
            {
                return new CreateUserResult
                {
                    Succeeded = false,
                    Errors = [$"User with user name '{email}' already exists."]
                };
            }
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                return new CreateUserResult
                {
                    Succeeded = false,
                    Errors = [$"Role '{role}' does not exist."]
                };
            }

            //create the user object
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                IsActive = true,
                EmailConfirmed = true
            };

            //create the user in the database and add to role
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return new CreateUserResult
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, role);
            if (!addToRoleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                return new CreateUserResult
                {
                    Succeeded = false,
                    Errors = addToRoleResult.Errors.Select(e => e.Description).ToList()
                };
            }

            return new CreateUserResult
            {
                Succeeded = true,
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Role = role,
                IsActive = user.IsActive
            };
        }
        public async Task<AuthResponseDTO> LoginAsync(string email, string password, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                throw new BusinessException("Invalid email or password.");

            if (user.IsActive == false)
                throw new BusinessException("User account is inactive.");

            var passwordValid = await _signInManager.CheckPasswordSignInAsync(
                user,
                password,
                lockoutOnFailure: false);

            if (!passwordValid.Succeeded)
                throw new BusinessException("Invalid email or password.");
            var activeTokens = await _context.RefreshTokens.Where(x => x.UserId == user.Id && x.RevokedAtUtc == null && x.ExpiresAtUtc > DateTime.UtcNow).ToListAsync(cancellationToken);
            foreach (var token in activeTokens)
            {
                token.RevokedAtUtc = DateTime.UtcNow;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var tokenResult = _jwtTokenGenerator.GenerateToken(user, roles);
            var refreshToken = new RefreshToken
            {
                RefreshTokenId = Guid.NewGuid(),
                Token = _refreshTokenGenerator.Generate(),
                UserId = user.Id,
                CreatedAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new AuthResponseDTO
            {
                AccessToken = tokenResult.AccessToken,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiresAtUtc = tokenResult.ExpiresAtUtc,
                RefreshTokenExpiresAtUtc = refreshToken.ExpiresAtUtc,
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                Roles = roles
            };
        }

        public async Task LogoutAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken, cancellationToken);
            if (token == null)
                return;
            if (!token.IsRevoked)
            {
                token.RevokedAtUtc = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var storedToken = await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == refreshToken, cancellationToken);
            if (storedToken == null)
                throw new BusinessException("Invalid refresh token.");

            if (storedToken.IsRevoked)
                throw new BusinessException("Refresh token has been revoked.");
            if (storedToken.IsExpired)
                throw new BusinessException("Refresh token has expired.");

            var user = storedToken.User;

            if (!user.IsActive)
                throw new BusinessException("User account is inactive.");

            var roles = await _userManager.GetRolesAsync(user);
            var jwtResult = _jwtTokenGenerator.GenerateToken(user, roles);

            storedToken.RevokedAtUtc = DateTime.UtcNow;

            var newRefreshToken = new RefreshToken
            {
                RefreshTokenId = Guid.NewGuid(),
                Token = _refreshTokenGenerator.Generate(),
                UserId = user.Id,
                CreatedAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new AuthResponseDTO
            {
                AccessToken = jwtResult.AccessToken,
                AccessTokenExpiresAtUtc = jwtResult.ExpiresAtUtc,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiresAtUtc = newRefreshToken.ExpiresAtUtc,
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                Roles = roles
            };
        }
        public async Task<CurrentUserDTO> GetCurrentUserAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new RecordNotFoundException(nameof(ApplicationUser), userId);
            var roles = await _userManager.GetRolesAsync(user);
            return new CurrentUserDTO
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Roles = roles
            };
        }
    }
}