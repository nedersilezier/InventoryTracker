using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Auth.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace InventoryTracker.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDTO>
    {
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<AuthResponseDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService
                .LoginAsync(request.Email, request.Password, cancellationToken);

            return result;
        }
    }
}