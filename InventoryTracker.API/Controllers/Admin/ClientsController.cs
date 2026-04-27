using InventoryTracker.Application.Features.Clients.Commands.ActivateClient;
using InventoryTracker.Application.Features.Clients.Commands.CreateClient;
using InventoryTracker.Application.Features.Clients.Commands.DeactivateClient;
using InventoryTracker.Application.Features.Clients.Commands.UpdateClient;
using InventoryTracker.Application.Features.Clients.Queries.GetById;
using InventoryTracker.Application.Features.Clients.Queries.GetClients;
using InventoryTracker.Application.Features.Clients.Queries.GetDetails;
using InventoryTracker.Contracts.Requests.Clients;
using InventoryTracker.Contracts.Responses.Clients;
using InventoryTracker.Contracts.Responses.Common;
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
        public async Task<IActionResult> GetAllClients([FromQuery] GetClientsRequest request, CancellationToken cancellationToken)
        {
            var query = new GetClientsQuery
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize ?? 10,
                SearchTerm = request.SearchTerm
            };
            var clientsPaged = await _mediator.Send(query, cancellationToken);
            var response = new PagedResponse<ClientResponseDTO>
            {
                Items = clientsPaged.Items.Select(client => new ClientResponseDTO
                {
                    ClientId = client.ClientId,
                    Name = client.Name,
                    ClientCode = client.ClientCode,
                    Email = client.Email,
                    PhoneNumber = client.PhoneNumber,
                    IsActive = client.IsActive,
                    Saldo = client.Saldo,
                    Address = new AddressResponseDTO
                    {
                        AddressId = client.Address.AddressId,
                        Street = client.Address.Street,
                        HouseNumber = client.Address.HouseNumber,
                        ApartmentNumber = client.Address.ApartmentNumber,
                        PostalCode = client.Address.PostalCode,
                        City = client.Address.City,
                        CountryId = client.Address.CountryId,
                        CountryName = client.Address.CountryName
                    }
                }).ToList(),
                TotalPages = clientsPaged.TotalPages,
                PageNumber = clientsPaged.PageNumber,
                PageSize = clientsPaged.PageSize,
                TotalCount = clientsPaged.TotalCount
            };
            return Ok(response);
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

        [HttpGet]
        [Route("{id}/details")]
        public async Task<IActionResult> GetClientDetailsById(Guid id, CancellationToken cancellationToken)
        {
            var client = await _mediator.Send(new GetClientDetailsByIdQuery(id), cancellationToken);
            if (client == null)
                return NotFound();
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient(CreateClientCommand command, CancellationToken cancellationToken)
        {
            var client = await _mediator.Send(command, cancellationToken);
            var response = new CreateClientResponse { ClientId = client.ClientId, ClientCode = client.ClientCode, ClientName = client.Name };
            return CreatedAtAction(nameof(GetClientById), new { id = client.ClientId }, response);
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
            var response = new CreateClientResponse { ClientId = client.ClientId, ClientCode = client.ClientCode, ClientName = client.Name };
            return Ok(response);
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
