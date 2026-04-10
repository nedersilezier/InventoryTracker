using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Auth.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly IIdentityService _identityService;
        public RefreshTokenCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.RefreshTokenAsync(request.RefreshToken, cancellationToken);
        }
    }
}
