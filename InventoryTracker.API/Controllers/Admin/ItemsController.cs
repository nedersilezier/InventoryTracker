using InventoryTracker.Application.Features.Items.Commands.CreateItem;
using InventoryTracker.Application.Features.Items.Commands.DeactivateItem;
using InventoryTracker.Application.Features.Items.Commands.UpdateItem;
using InventoryTracker.Application.Features.Items.Queries.GetItems;
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
        public async Task<IActionResult> GetAllItems(CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetItemsQuery(), cancellationToken);
            return Ok(items);
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
        [HttpPost]
        public async Task<IActionResult> CreateItem(CreateItemCommand command, CancellationToken cancellationToken)
        {
            var item = await _mediator.Send(command, cancellationToken);
            return Ok(item);
        }
        [HttpPut]
        [Route("{id}/update")]
        public async Task<IActionResult> UpdateItem(Guid id, UpdateItemCommand command, CancellationToken cancellationToken)
        {
            if (id != command.ItemId)
                return BadRequest("ID in URL does not match ID in body.");
            var item = await _mediator.Send(command, cancellationToken);
            if (item == null)
                return NotFound();
            return Ok(item);
        }
        [HttpDelete]
        [Route("{id}/deactivate")]
        public async Task<IActionResult> DeactivateItem(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeactivateItemCommand(id), cancellationToken);
            if (!result)
                return NotFound();
            return Ok();
        }
    }
}
