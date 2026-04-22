using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Transactions.Queries.GetTransactions
{
    public class GetRecentTransactionsQueryValidator: AbstractValidator<GetRecentTransactionsQuery>
    {
        public GetRecentTransactionsQueryValidator()
        {
            RuleFor(x => x.Count)
                .GreaterThan(0).WithMessage("Count must be greater than 0.");
        }
    }
}
