using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Countries.Commands.CreateCountry
{
    public class CreateCountryCommandValidator: AbstractValidator<CreateCountryCommand>
    {
        public CreateCountryCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Country name is required.")
                .MaximumLength(100).WithMessage("Country name must not exceed 100 characters.");
            RuleFor(c => c.Code)
                .NotEmpty().WithMessage("Country code is required.")
                .MaximumLength(3).WithMessage("Country code must not exceed 3 characters.");
        }
    }
}
