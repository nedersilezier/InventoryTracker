using Azure.Core;
using InventoryTracker.Application.Features.Items.Commands.ActivateItem;
using InventoryTracker.Application.Features.Items.Commands.CreateItem;
using InventoryTracker.Application.Features.Items.Commands.DeactivateItem;
using InventoryTracker.Application.Features.Items.Commands.UpdateItem;
using InventoryTracker.Application.Features.Items.Queries.GetItems;
using InventoryTracker.Application.Features.Warehouses.Queries.GetWarehouses;
using InventoryTracker.Contracts.Requests.Items;
using InventoryTracker.Contracts.Responses.Clients;
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
    [Route("api/admin/items")]
    [Authorize(Roles = "Admin")]
    public class ItemsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllItems([FromQuery] GetItemsRequest request, CancellationToken cancellationToken)
        {
            var query = new GetItemsQuery
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize ?? 10,
                SearchTerm = request.SearchTerm
            };
            var itemsPaged = await _mediator.Send(query, cancellationToken);
            var response = new PagedResponse<ItemResponseDTO>
            {
                Items = itemsPaged.Items.Select(i => new ItemResponseDTO
                {
                    ItemId = i.ItemId,
                    Name = i.Name,
                    SKU = i.SKU,
                    UnitOfMeasure = i.UnitOfMeasure,
                    Weight = i.Weight,
                    CreditValue = i.CreditValue,
                    Description = i.Description,
                    IsActive = i.IsActive,
                }).ToList(),
                TotalPages = itemsPaged.TotalPages,
                PageNumber = itemsPaged.PageNumber,
                PageSize = itemsPaged.PageSize,
                TotalCount = itemsPaged.TotalCount,
                TotalActive = itemsPaged.TotalActive
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetItemById(Guid id, CancellationToken cancellationToken)
        {
            var item = await _mediator.Send(new GetItemByIdQuery(id), cancellationToken);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpGet]
        [Route("{id}/details")]
        public async Task<IActionResult> GetItemDetailsById(Guid id, CancellationToken cancellationToken)
        {
            var item = await _mediator.Send(new GetItemDetailsByIdQuery(id), cancellationToken);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(CreateItemCommand command, CancellationToken cancellationToken)
        {
            var item = await _mediator.Send(command, cancellationToken);
            var response = new CreateItemResponse { ItemId = item.ItemId, Name = item.Name, SKU = item.SKU };
            return CreatedAtAction(nameof(GetItemById), new { id = item.ItemId }, response);
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, UpdateItemRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateItemCommand
            {
                ItemId = id,
                Name = request.Name,
                SKU = request.SKU,
                Description = request.Description,
                UnitOfMeasure = request.UnitOfMeasure,
                CreditValue = request.CreditValue,
                Weight = request.Weight
            };
            var item = await _mediator.Send(command, cancellationToken);
            if (item == null)
                return NotFound();
            var response = new CreateItemResponse { ItemId = item.ItemId, Name = item.Name, SKU = item.SKU };
            return Ok(response);
        }
       
        [HttpPatch]
        [Route("{id}/deactivate")]
        public async Task<IActionResult> DeactivateItem(Guid id, CancellationToken cancellationToken)
        {
            var item = await _mediator.Send(new DeactivateItemCommand(id), cancellationToken);
            if (item == null)
                return NotFound();
            return Ok(item);
        }
        [HttpPatch]
        [Route("{id}/activate")]
        public async Task<IActionResult> ActivateItem(Guid id, CancellationToken cancellationToken)
        {
            var item = await _mediator.Send(new ActivateItemCommand(id), cancellationToken);
            if (item == null)
                return NotFound();
            return Ok(item);
        }
    }
}
