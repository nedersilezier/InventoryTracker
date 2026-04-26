using InventoryTracker.Application.Features.Warehouses.Commands.ActivateWarehouse;
using InventoryTracker.Application.Features.Warehouses.Commands.CreateWarehouse;
using InventoryTracker.Application.Features.Warehouses.Commands.DeactivateWarehouse;
using InventoryTracker.Application.Features.Warehouses.Commands.UpdateWarehouse;
using InventoryTracker.Application.Features.Warehouses.Queries.GetWarehouses;
using InventoryTracker.Contracts.Requests.Warehouses;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Items;
using InventoryTracker.Contracts.Responses.Warehouses;
using InventoryTracker.Domain.Entities;
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
        public async Task<IActionResult> GetAllWarehouses([FromQuery] GetWarehousesRequest request, CancellationToken cancellationToken)
        {
            var query = new GetWarehousesQuery
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize ?? 10,
                SearchTerm = request.SearchTerm
            };
            var warehousesPaged = await _mediator.Send(query, cancellationToken);
            var response = new PagedResponse<WarehouseResponseDTO>
            {
                Items = warehousesPaged.Items.Select(i => new WarehouseResponseDTO
                {
                    WarehouseId = i.WarehouseId,
                    Name = i.Name,
                    Code = i.Code,
                    StockCount = i.StocksCount,
                    IsActive = i.IsActive,
                    Address = new AddressResponseDTO
                    {
                        AddressId = i.Address.AddressId,
                        Street = i.Address.Street,
                        HouseNumber = i.Address.HouseNumber,
                        ApartmentNumber = i.Address.ApartmentNumber,
                        PostalCode = i.Address.PostalCode,
                        City = i.Address.City,
                        CountryId = i.Address.CountryId,
                        CountryName = i.Address.CountryName
                    }
                }).ToList(),
                TotalPages = warehousesPaged.TotalPages,
                PageNumber = warehousesPaged.PageNumber,
                PageSize = warehousesPaged.PageSize,
                TotalCount = warehousesPaged.TotalCount
            };  
            return Ok(response);
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

        [HttpGet]
        [Route("{id}/details")]
        public async Task<IActionResult> GetWarehouseDetailsById(Guid id, CancellationToken cancellationToken)
        {
            var warehouse = await _mediator.Send(new GetWarehouseDetailsByIdQuery(id), cancellationToken);
            if (warehouse == null)
                return NotFound();
            return Ok(warehouse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouse(CreateWarehouseCommand command, CancellationToken cancellationToken)
        {
            var warehouse = await _mediator.Send(command, cancellationToken);
            var response = new CreateWarehouseResponse { WarehouseId = warehouse.WarehouseId, Name = warehouse.Name, Code = warehouse.Code };
            return CreatedAtAction(nameof(GetWarehouseById), new { id = warehouse.WarehouseId }, response);
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
            var response = new CreateWarehouseResponse { WarehouseId = warehouse.WarehouseId, Name = warehouse.Name, Code = warehouse.Code };
            return Ok(response);
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
