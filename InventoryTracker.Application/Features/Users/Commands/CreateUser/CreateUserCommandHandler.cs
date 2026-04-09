using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Users.Commands.CreateUser;
using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDTO>
    {
        private readonly IIdentityService _identityService;

        public CreateUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.CreateUserAsync(request, cancellationToken);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors);
                throw new InvalidOperationException($"User creation failed: {errors}");
            }

            return new UserDTO
            {
                UserId = result.UserId!,
                Email = result.Email!,
                UserName = result.UserName!,
                FirstName = result.FirstName!,
                LastName = result.LastName!,
                PhoneNumber = result.PhoneNumber!,
                Role = result.Role!,
                IsActive = result.IsActive
            };
        }
    }
}