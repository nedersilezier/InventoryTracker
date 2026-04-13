using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Users.Commands.CreateUser;
using InventoryTracker.Application.Features.Users.Queries.GetUsers;
using InventoryTracker.Contracts.Requests.Users;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Users;
using InventoryTracker.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class UsersController:ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] GetUsersRequest request, CancellationToken cancellationToken)
        {
            var query = new GetUsersQuery
            {
                PageNumber = request.PageNumber,
                //test
                PageSize = request.PageSize ?? 1,
                SearchTerm = request.SearchTerm
            };
            var usersPaged = await _mediator.Send(query, cancellationToken);
            var response = new PagedResponse<UserResponseDTO>
            {
                Items = usersPaged.Items.Select(u => new UserResponseDTO
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    IsActive = u.IsActive,
                    UserName = u.UserName,
                    Role = u.Role
                }).ToList(),

                TotalPages = usersPaged.TotalPages,
                PageNumber = usersPaged.PageNumber,
                PageSize = usersPaged.PageSize,
                TotalCount = usersPaged.TotalCount
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            //To be changed to:
            //return CreatedAtAction(nameof(GetUserById), new { id = result.UserId }, result);

            return Ok(result);
        }
    }
}
