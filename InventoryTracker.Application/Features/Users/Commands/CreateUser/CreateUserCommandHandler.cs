using FluentValidation;
using FluentValidation.Results;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Users.Commands.CreateUser;
using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserCreatedDTO>
    {
        private readonly IIdentityService _identityService;

        public CreateUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UserCreatedDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService
                .CreateUserAsync(
                request.FirstName, 
                request.LastName, 
                request.Email, 
                request.Password, 
                request.PhoneNumber,
                request.Role);

            if (!result.Succeeded)
            {
                var failures = result.Errors.Select(error => new ValidationFailure("General", error)).ToList();
                throw new ValidationException(failures);
            }

            return new UserCreatedDTO
            {
                UserId = result.UserId,
                Email = result.Email!
            };
        }
    }
}