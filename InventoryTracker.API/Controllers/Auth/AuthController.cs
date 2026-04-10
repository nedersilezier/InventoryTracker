using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Auth.Commands.Login;
using InventoryTracker.Application.Features.Auth.Commands.RefreshToken;
using InventoryTracker.Application.Features.Auth.DTOs;
using InventoryTracker.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.API.Controllers.Auth
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken(RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
