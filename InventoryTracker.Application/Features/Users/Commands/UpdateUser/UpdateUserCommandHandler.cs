using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;
using FluentValidation;
using FluentValidation.Results;

namespace InventoryTracker.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserCreatedDTO>
    {
        private readonly IIdentityService _identityService;

        public UpdateUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UserCreatedDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.UpdateUserAsync(request.UserId, request.FirstName, request.LastName, request.PhoneNumber, request.Role);

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
