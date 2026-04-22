using InventoryTracker.Application.Features.Warehouses.Queries.GetWarehouses;
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
        public IActionResult Index()
        {
            return View();
        }
    }
}
