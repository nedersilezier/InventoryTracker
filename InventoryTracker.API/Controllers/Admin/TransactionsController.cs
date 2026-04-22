using InventoryTracker.Application.Features.Transactions.Commands.ApproveTransaction;
using InventoryTracker.Application.Features.Transactions.Commands.CancelTransaction;
using InventoryTracker.Application.Features.Transactions.Commands.CreateTransaction;
using InventoryTracker.Application.Features.Transactions.Commands.UpdateTransaction;
using InventoryTracker.Application.Features.Transactions.Queries.GetTransactions;
using InventoryTracker.Contracts.Requests.Transactions;
using InventoryTracker.Contracts.Responses.Clients;
using InventoryTracker.Contracts.Responses.Common;
using InventoryTracker.Contracts.Responses.Transactions;
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
        [Route("{count}")]
        public async Task<IActionResult> GetTransactionById(Guid id, CancellationToken cancellationToken)
        {
            var transaction = await _mediator.Send(new GetTransactionByIdQuery(id), cancellationToken);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpGet]
        [Route("recent/{count}")]
        public async Task<IActionResult> GetRecentTransactions(int count, CancellationToken cancellationToken)
        {
            var query = new GetRecentTransactionsQuery { Count = count };
            var transactions = await _mediator.Send(query, cancellationToken);
            return Ok(transactions);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(CreateTransactionCommand command, CancellationToken cancellationToken)
        {
            var transactionId = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetTransactionById), new { id = transactionId }, null);
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
        [HttpPatch]
        [Route("{id}/cancel")]
        public async Task<IActionResult> CancelTransaction(Guid id, CancelTransactionRequest request, CancellationToken cancellationToken)
        {
            var command = new CancelTransactionCommand
            {
                TransactionId = id,
                CancellationReason = request.CancellationReason
            };
            var result = await _mediator.Send(command, cancellationToken);
            if (result == Guid.Empty)
                return NotFound();
            return Ok(result);
        }
        [HttpPatch]
        [Route("{id}/approve")]
        public async Task<IActionResult> ApproveTransaction(Guid id, CancellationToken cancellationToken)
        {
            var command = new ApproveTransactionCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            if (result == Guid.Empty)
                return NotFound();
            return Ok(result);
        }
    }
}