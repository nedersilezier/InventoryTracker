using InventoryTracker.Contracts.Requests.Warehouses;
using InventoryTracker.Application.Features.Warehouses.Commands.ActivateWarehouse;
using InventoryTracker.Application.Features.Warehouses.Commands.CreateWarehouse;
using InventoryTracker.Application.Features.Warehouses.Commands.DeactivateWarehouse;
using InventoryTracker.Application.Features.Warehouses.Commands.UpdateWarehouse;
using InventoryTracker.Application.Features.Warehouses.Queries.GetWarehouses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/warehouses")]
    [Authorize(Roles = "Admin")]
    public class WarehousesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public WarehousesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWarehouses(CancellationToken cancellationToken)
        {
            var warehouses = await _mediator.Send(new GetWarehousesQuery(), cancellationToken);
            return Ok(warehouses);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetWarehouseById(Guid id, CancellationToken cancellationToken)
        {
            var warehouse = await _mediator.Send(new GetWarehouseByIdQuery(id), cancellationToken);
            if (warehouse == null)
                return NotFound();
            return Ok(warehouse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouse(CreateWarehouseCommand command, CancellationToken cancellationToken)
        {
            var warehouse = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetWarehouseById), new { id = warehouse.WarehouseId }, warehouse);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateWarehouse(Guid id, UpdateWarehouseRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateWarehouseCommand
            {
                WarehouseId = id,
                Name = request.Name,
                Code = request.Code,
                Address = new UpdateWarehouseCommand.UpdateWarehouseAddressDTO
                {
                    Street = request.Address.Street,
                    HouseNumber = request.Address.HouseNumber,
                    ApartmentNumber = request.Address.ApartmentNumber,
                    PostalCode = request.Address.PostalCode,
                    City = request.Address.City,
                    CountryId = request.Address.CountryId
                }
            };
            var warehouse = await _mediator.Send(command, cancellationToken);
            if (warehouse == null)
                return NotFound();
            return Ok(warehouse);
        }

        [HttpPatch]
        [Route("{id}/deactivate")]
        public async Task<IActionResult> DeactivateWarehouse(Guid id, CancellationToken cancellationToken)
        {
            var warehouse = await _mediator.Send(new DeactivateWarehouseCommand(id), cancellationToken);
            if (warehouse == null)
                return NotFound();
            return Ok(warehouse);
        }

        [HttpPatch]
        [Route("{id}/activate")]
        public async Task<IActionResult> ActivateWarehouse(Guid id, CancellationToken cancellationToken)
        {
            var warehouse = await _mediator.Send(new ActivateWarehouseCommand(id), cancellationToken);
            if (warehouse == null)
                return NotFound();
            return Ok(warehouse);
        }
    }
}
