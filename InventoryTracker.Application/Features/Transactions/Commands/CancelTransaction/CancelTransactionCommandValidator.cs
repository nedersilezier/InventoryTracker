using FluentValidation;
using InventoryTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;


namespace InventoryTracker.Application.Features.Transactions.Commands.CancelTransaction
{
    public class CancelTransactionCommandValidator: AbstractValidator<CancelTransactionCommand>
    {
        public CancelTransactionCommandValidator()
        {
            RuleFor(x => x.TransactionId)
                .NotEmpty().WithMessage("TransactionId is required.");
            RuleFor(x => x.CancellationReason)
                .MaximumLength(500).WithMessage("Cancellation reason cannot exceed 500 characters.");
        }
    }
}
