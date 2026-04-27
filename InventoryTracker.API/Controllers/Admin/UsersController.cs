using InventoryTracker.Application.Features.Items.Queries.GetItems;
using InventoryTracker.Application.Features.Users.Commands.ActivateUser;
using InventoryTracker.Application.Features.Users.Commands.CreateUser;
using InventoryTracker.Application.Features.Users.Commands.DeactivateUser;
using InventoryTracker.Application.Features.Users.Commands.UpdateUser;
using InventoryTracker.Application.Features.Users.Queries.GetById;
using InventoryTracker.Application.Features.Users.Queries.GetRoles;
using InventoryTracker.Application.Features.Users.Queries.GetUsers;
using InventoryTracker.Contracts.Requests.Users;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
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
                    Roles = u.Roles
                }).ToList(),

                TotalPages = usersPaged.TotalPages,
                PageNumber = usersPaged.PageNumber,
                PageSize = usersPaged.PageSize,
                TotalCount = usersPaged.TotalCount
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserById(string id, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id), cancellationToken);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet]
        [Route("roles")]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
        {
            var roles = await _mediator.Send(new GetRolesQuery(), cancellationToken);
            var response = roles.Select(r => new RoleResponseDTO { Name = r.Name });
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            var response = new UserCreatedResponse { UserId = result.UserId!, Email = result.Email };
            //To be changed to:
            //return CreatedAtAction(nameof(GetUserById), new { id = result.UserId }, result);

            return Ok(response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateUserCommand
            {
                UserId = id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Role = request.Role
            };
            var result = await _mediator.Send(command, cancellationToken);
            if (result == null)
                return NotFound();

            var response = new UserCreatedResponse { UserId = result.UserId!, Email = result.Email };
            //To be changed to:
            //return CreatedAtAction(nameof(GetUserById), new { id = result.UserId }, result);

            return Ok(response);
        }

        [HttpPatch]
        [Route("{id}/activate")]
        public async Task<IActionResult> ActivateUser(string id, CancellationToken cancellationToken)
        {
            var command = new ActivateUserCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            if (result == null)
                return NotFound();

            var response = new UserCreatedResponse { UserId = result.UserId!, Email = result.Email };
            //To be changed to:
            //return CreatedAtAction(nameof(GetUserById), new { id = result.UserId }, result);

            return Ok(response);
        }

        [HttpPatch]
        [Route("{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(string id, CancellationToken cancellationToken)
        {
            var command = new DeactivateUserCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            if (result == null)
                return NotFound();

            var response = new UserCreatedResponse { UserId = result.UserId!, Email = result.Email };
            //To be changed to:
            //return CreatedAtAction(nameof(GetUserById), new { id = result.UserId }, result);

            return Ok(response);
        }
    }
}
