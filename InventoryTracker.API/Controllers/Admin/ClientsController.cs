using InventoryTracker.API.Requests.Clients;
using InventoryTracker.Application.Features.Clients.Commands.ActivateClient;
using InventoryTracker.Application.Features.Clients.Commands.CreateClient;
using InventoryTracker.Application.Features.Clients.Commands.DeactivateClient;
using InventoryTracker.Application.Features.Clients.Commands.UpdateClient;
using InventoryTracker.Application.Features.Clients.Queries.GetClients;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/clients")]
    [Authorize(Roles = "Admin")]
    public class ClientsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ClientsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllClients(CancellationToken cancellationToken)
        {
            var clients = await _mediator.Send(new GetClientsQuery(), cancellationToken);
            return Ok(clients);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetClientById(Guid id, CancellationToken cancellationToken)
        {
            var client = await _mediator.Send(new GetClientByIdQuery(id), cancellationToken);
            if (client == null)
                return NotFound();
            return Ok(client);
        }
        [HttpPost]
        public async Task<IActionResult> CreateClient(CreateClientCommand command, CancellationToken cancellationToken)
        {
            var client = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetClientById), new { id = client.ClientId }, client);
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateClient(Guid id, UpdateClientRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateClientCommand
            {
                ClientId = id,
                Name = request.Name,
                ClientCode = request.ClientCode,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = new UpdateClientCommand.UpdateClientAddressDTO
                {
                    Street = request.Address.Street,
                    HouseNumber = request.Address.HouseNumber,
                    ApartmentNumber = request.Address.ApartmentNumber,
                    PostalCode = request.Address.PostalCode,
                    City = request.Address.City,
                    CountryId = request.Address.CountryId
                }
            };
            var client = await _mediator.Send(command, cancellationToken);
            if (client == null)
                return NotFound();
            return Ok(client);
        }

        [HttpPatch]
        [Route("{id}/deactivate")]
        public async Task<IActionResult> DeleteClient(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeactivateClientCommand(id), cancellationToken);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        [HttpPatch]
        [Route("{id}/activate")]
        public async Task<IActionResult> ActivateClient(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ActivateClientCommand(id), cancellationToken);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}
