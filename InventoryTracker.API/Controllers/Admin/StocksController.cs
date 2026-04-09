using InventoryTracker.Application.Features.Stocks.Queries.GetStocksByWarehouse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/stocks")]
    [Authorize(Roles = "Admin")]
    public class StocksController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StocksController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Route("warehouse/{warehouseId}")]
        public async Task<IActionResult> GetStocksByWarehouseId(Guid warehouseId, CancellationToken cancellationToken)
        {
            var stocks = await _mediator.Send(new GetStocksByWarehouseIdQuery(warehouseId), cancellationToken);
            return Ok(stocks);
        }
    }
}
