using FluentValidation;
using InventoryTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid transaction type.");

            RuleFor(x => x.TransactionDate)
                    .NotEmpty().WithMessage("Transaction date is required.")
                    .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Transaction date cannot be in the future.");

            RuleFor(x => x.ReferenceNumber)
                    .MaximumLength(100).WithMessage("Reference number cannot exceed 100 characters.");

            RuleFor(x => x.Notes)
                    .MaximumLength(1000)
                    .WithMessage("Notes cannot exceed 1000 characters");

            RuleFor(x => x.Items)
                    .NotNull().WithMessage("Items are required.")
                    .NotEmpty().WithMessage("At least one transaction item is required.");

            RuleForEach(x => x.Items)
            .ChildRules(items =>
            {
                items.RuleFor(i => i.ItemId)
                    .NotEmpty().WithMessage("ItemId is required.");
            });

            RuleFor(x => x.Items)
                .Must(items => items == null || items.GroupBy(i => i.ItemId).All(g => g.Count() == 1)).WithMessage("Duplicate items are not allowed.");

            RuleFor(x => x).Custom((request, context) =>
            {
                foreach (var item in request.Items)
                {
                    if (request.Type == TransactionType.Adjustment)
                    {
                        if (item.Quantity == 0)
                        {
                            context.AddFailure("Adjustment quantity cannot be zero.");
                        }
                    }
                    else
                    {
                        if (item.Quantity <= 0)
                        {
                            context.AddFailure("Quantity must be greater than zero.");
                        }
                    }
                }
                switch (request.Type)
                {
                    case TransactionType.IssueToClient:
                        if (!request.ClientId.HasValue || !request.SourceWarehouseId.HasValue || request.DestinationWarehouseId.HasValue)
                        {
                            context.AddFailure("IssueToClient requires SourceWarehouseId and ClientId and cannot contain DestinationWarehouseId");
                        }
                        break;

                    case TransactionType.ReturnFromClient:
                        if (!request.ClientId.HasValue || !request.DestinationWarehouseId.HasValue || request.SourceWarehouseId.HasValue)
                        {
                            context.AddFailure("ReturnFromClient requires ClientId and DestinationWarehouseId and cannot contain SourceWarehouseId.");
                        }
                        break;

                    case TransactionType.TransferBetweenWarehouses:
                        if (!request.SourceWarehouseId.HasValue || !request.DestinationWarehouseId.HasValue || request.ClientId.HasValue)
                        {
                            context.AddFailure("TransferBetweenWarehouses requires SourceWarehouseId and DestinationWarehouseId and cannot contain ClientId.");
                        }
                        break;

                    case TransactionType.Adjustment:
                        if (request.ClientId.HasValue)
                            context.AddFailure("Adjustment cannot involve clients.");
                        if (request.DestinationWarehouseId.HasValue)
                            context.AddFailure("Adjustment cannot involve destination warehouses.");
                        if (!request.SourceWarehouseId.HasValue)
                            context.AddFailure("Adjustment requires a source warehouse.");
                        break;

                    default:
                        context.AddFailure("Unsupported transaction type.");
                        break;
                }
            });
        }
    }
}