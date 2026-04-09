using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Transactions.Commands.ApproveTransaction
{
    public class ApproveTransactionCommandValidator: AbstractValidator<ApproveTransactionCommand>
    {
        public ApproveTransactionCommandValidator()
        {
                RuleFor(x => x.TransactionId)
                    .NotEmpty().WithMessage("TransactionId is required.");
        }
    }
}
