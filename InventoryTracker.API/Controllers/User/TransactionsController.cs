using InventoryTracker.Application.Features.Transactions.Commands.ApproveTransaction;
using InventoryTracker.Application.Features.Transactions.Commands.CancelTransaction;
using InventoryTracker.Application.Features.Transactions.Commands.CreateTransaction;
using InventoryTracker.Application.Features.Transactions.Commands.UpdateTransaction;
using InventoryTracker.Application.Features.Transactions.Queries.GetTransactions;
using InventoryTracker.Contracts.Requests.Transactions;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Transactions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.API.Controllers.User
{
    [ApiController]
    [Route("api/user/transactions")]
    [Authorize(Roles = "User")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions([FromQuery] GetTransactionsRequest request, CancellationToken cancellationToken)
        {
            var query = new GetTransactionsQuery
            {
                SearchTerm = request.SearchTerm,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                IncludeAdjustments = request.IncludeAdjustments,
                IncludeTransfers = request.IncludeTransfers,
                IncludeIssues = request.IncludeIssues,
                IncludeReturns = request.IncludeReturns,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo
            };
            var transactionsPaged = await _mediator.Send(query, cancellationToken);
            var response = new PagedResponse<TransactionListDTO>
            {
                Items = transactionsPaged.Items.Select(transaction => new TransactionListDTO
                {
                    TransactionId = transaction.TransactionId,
                    Type = transaction.Type,
                    Status = transaction.Status,
                    TransactionDate = transaction.TransactionDate,
                    ClientId = transaction.ClientId,
                    ClientName = transaction.ClientName,
                    SourceWarehouseId = transaction.SourceWarehouseId,
                    DestinationWarehouseId = transaction.DestinationWarehouseId,
                    SourceWarehouseNameSnapshot = transaction.SourceWarehouseNameSnapshot,
                    DestinationWarehouseNameSnapshot = transaction.DestinationWarehouseNameSnapshot,
                    ReferenceNumber = transaction.ReferenceNumber,
                    FromDisplay = transaction.FromDisplay,
                    ToDisplay = transaction.ToDisplay
                }).ToList(),
                TotalPages = transactionsPaged.TotalPages,
                PageNumber = transactionsPaged.PageNumber,
                PageSize = transactionsPaged.PageSize,
                TotalCount = transactionsPaged.TotalCount
            };
            return Ok(response);
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
            var transactionId = await _mediator.Send(command, cancellationToken);
            var response = new CreateTransactionResponse { TransactionId = transactionId };
            return CreatedAtAction(nameof(GetTransactionById), new { id = transactionId }, response);
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
            if (transaction == Guid.Empty)
                return NotFound();
            return Ok(transaction);
        }
    }
}