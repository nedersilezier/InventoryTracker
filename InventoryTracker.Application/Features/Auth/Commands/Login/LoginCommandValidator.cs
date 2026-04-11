using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Auth.Commands.Login
{
    public class LoginCommandValidator: AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(256).WithMessage("Email cannot exceed 256 characters.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
