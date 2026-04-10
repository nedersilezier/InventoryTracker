using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommand: IRequest
    {
        public string RefreshToken { get; set; } = default!;
    }
}
