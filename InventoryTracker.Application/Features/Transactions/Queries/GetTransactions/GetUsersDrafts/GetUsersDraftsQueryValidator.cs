using FluentValidation;

namespace InventoryTracker.Application.Features.Transactions.Queries.GetTransactions.GetUsersDrafts
{
    public class GetUsersDraftsQueryValidator : AbstractValidator<GetUsersDraftsQuery>
    {
        public GetUsersDraftsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");
            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.");
            RuleFor(x => x.DateTo)
                 .Must((model, dateTo) =>
                     !model.DateFrom.HasValue ||
                     !dateTo.HasValue ||
                     model.DateFrom.Value.Date <= dateTo.Value.Date)
                 .WithMessage("Date to must be greater than or equal to date from.");
        }
    }
}
