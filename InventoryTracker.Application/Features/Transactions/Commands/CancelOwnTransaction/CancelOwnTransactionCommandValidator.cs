using FluentValidation;
using InventoryTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;


namespace InventoryTracker.Application.Features.Transactions.Commands.CancelOwnTransaction
{
    public class CancelOwnTransactionCommandValidator<T> : AbstractValidator<CancelOwnTransactionCommand>
    {
        public CancelOwnTransactionCommandValidator()
        {
            RuleFor(x => x.TransactionId)
                .NotEmpty().WithMessage("TransactionId is required.");
            RuleFor(x => x.CancellationReason)
                .MaximumLength(500).WithMessage("Cancellation reason cannot exceed 500 characters.");
        }
    }
}
