using InventoryTracker.Application.Features.Auth.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<AuthResponseDto>
    {
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
    }
}
