using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Transactions.Commands
{
    public class CreateTransactionCommandValidator: AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(x => x.Type.ToString())
                    .NotEmpty().WithMessage("Type is required.")
                    .MaximumLength(50);

            RuleFor(x => x.TransactionDate)
                    .NotEmpty().WithMessage("Transaction date is required.")
                    .LessThanOrEqualTo(DateTime.Now).WithMessage("Transaction date cannot be in the future.");

            RuleFor(x => x.ReferenceNumber)
                    .MaximumLength(100).WithMessage("Reference number cannot exceed 100 characters.");

            RuleFor(x => x.Notes)
                    .MaximumLength(1000)
                    .WithMessage("Notes cannot exceed 1000 characters");

            RuleFor(x => x.Items)
                    .NotEmpty().WithMessage("At least one transaction item is required.");
        }
    }
}
