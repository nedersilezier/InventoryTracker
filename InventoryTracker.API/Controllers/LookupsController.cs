using InventoryTracker.Application.Features.Clients.Queries.GetLookups;
using InventoryTracker.Application.Features.Countries.Queries.GetCountries;
using InventoryTracker.Application.Features.Items.Queries.GetLookups;
using InventoryTracker.Application.Features.Warehouses.Queries.GetLookups;
using InventoryTracker.Contracts.Responses.Clients;
using InventoryTracker.Contracts.Responses.Countries;
using InventoryTracker.Contracts.Responses.Items;
using InventoryTracker.Contracts.Responses.Warehouses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class LookupsController : Controller
    {
        private readonly IMediator _mediator;
        public LookupsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("warehouses")]
        public async Task<IActionResult> GetActiveWarehouses(CancellationToken cancellationToken)
        {
            var warehouses = await _mediator.Send(new GetWarehousesSelectQuery(), cancellationToken);
            var response = warehouses.Select(w => new WarehouseResponseSelectDTO
            {
                WarehouseId = w.WarehouseId,
                Name = w.Name
            }).ToList();
            return Ok(response);
        }
        [HttpGet("countries")]
        public async Task<IActionResult> GetActiveCountries(CancellationToken cancellationToken)
        {
            var countries = await _mediator.Send(new GetCountriesSelectQuery(), cancellationToken);
            var response = countries.Select(c => new CountryResponseSelectDTO
            {
                CountryId = c.CountryId,
                CountryName = c.CountryName
            }).ToList();
            return Ok(response);
        }

        [HttpGet("clients")]
        public async Task<IActionResult> GetActiveClients(CancellationToken cancellationToken)
        {
            var clients = await _mediator.Send(new GetClientsSelectQuery(), cancellationToken);
            var response = clients.Select(c => new ClientResponseSelectDTO
            {
                ClientId = c.ClientId,
                Name = c.Name
            }).ToList();
            return Ok(response);
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetActiveItems(CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetItemsSelectQuery(), cancellationToken);
            var response = items.Select(i => new ItemResponseSelectDTO
            {
                ItemId = i.ItemId,
                Name = i.Name,
                UnitOfMeasure = i.UnitOfMeasure,
            }).ToList();
            return Ok(response);
        }
    }
}
