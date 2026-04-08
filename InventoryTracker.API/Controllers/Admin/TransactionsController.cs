using InventoryTracker.API.Requests.Transactions;
using InventoryTracker.Application.Features.Transactions.Commands.CreateTransaction;
using InventoryTracker.Application.Features.Transactions.Commands.UpdateTransaction;
using InventoryTracker.Application.Features.Transactions.Queries.GetTransactions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/transactions")]
    [Authorize(Roles = "Admin")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions(CancellationToken cancellationToken)
        {
            var transactions = await _mediator.Send(new GetTransactionsQuery(), cancellationToken);
            return Ok(transactions);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetTransactionById(Guid id, CancellationToken cancellationToken)
        {
            var transaction = await _mediator.Send(new GetTransactionByIdQuery(id), cancellationToken);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(CreateTransactionCommand command, CancellationToken cancellationToken)
        {
            var transaction = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.TransactionId }, transaction);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateTransaction(Guid id, UpdateTransactionRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateTransactionCommand
            {
                TransactionId = id,
                Type = request.Type,
                ClientId = request.ClientId,
                SourceWarehouseId = request.SourceWarehouseId,
                DestinationWarehouseId = request.DestinationWarehouseId,
                TransactionDate = request.TransactionDate,
                ReferenceNumber = request.ReferenceNumber,
                Notes = request.Notes,
                Items = request.Items.Select(i => new UpdateTransactionCommand.UpdateTransactionItemDTO
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity
                }).ToList()
            };
            var transaction = await _mediator.Send(command, cancellationToken);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }
    }
}
