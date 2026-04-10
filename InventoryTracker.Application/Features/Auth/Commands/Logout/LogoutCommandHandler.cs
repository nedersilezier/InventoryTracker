using InventoryTracker.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommandHandler: IRequestHandler<LogoutCommand>
    {
        private readonly IIdentityService _identityService;
        public LogoutCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            // Invalidate the refresh token
            await _identityService.LogoutAsync(request.RefreshToken, cancellationToken);
        }
    }
}
