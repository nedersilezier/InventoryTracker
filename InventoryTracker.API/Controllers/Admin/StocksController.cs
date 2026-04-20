using InventoryTracker.Application.Features.Items.Queries.GetItems;
using InventoryTracker.Application.Features.Stocks.Queries.GetStocksByWarehouse;
using InventoryTracker.Contracts.Requests.Items;
using InventoryTracker.Contracts.Requests.Stocks;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Items;
using InventoryTracker.Contracts.Responses.Stocks;
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
        [Route("warehouse")]
        public async Task<IActionResult> GetStocksAsync([FromQuery] GetStocksRequest request, CancellationToken cancellationToken)
        {
            var query = new GetStocksQuery
            {
                WarehouseId = request.WarehouseId,
                PageNumber = request.PageNumber ?? 1,
                PageSize = request.PageSize ?? 10,
                SearchTerm = request.SearchTerm
            };
            var stocksPaged = await _mediator.Send(query, cancellationToken);
            var response = new PagedResponse<StocksResponseDTO>
            {
                Items = stocksPaged.Items.Select(s => new StocksResponseDTO
                {
                    StockId = s.StockId,
                    WarehouseId = s.WarehouseId,
                    ItemId = s.ItemId,
                    ItemName = s.ItemName,
                    WarehouseName = s.WarehouseName,
                    SKU = s.SKU,
                    UnitOfMeasure = s.UnitOfMeasure,
                    Weight = s.Weight,
                    CreditValue = s.CreditValue,
                    Quantity = s.Quantity
                }).ToList(),
                TotalPages = stocksPaged.TotalPages,
                PageNumber = stocksPaged.PageNumber,
                PageSize = stocksPaged.PageSize,
                TotalCount = stocksPaged.TotalCount
            };
            return Ok(response);
        }
    }
}
