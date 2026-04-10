using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandValidator: AbstractValidator<RefreshTokenCommand>
    {
            public RefreshTokenCommandValidator()
            {
                RuleFor(x => x.RefreshToken)
                    .NotEmpty().WithMessage("Refresh token is required.")
                    .MaximumLength(512).WithMessage("Refresh token must not exceed 512 characters.");
        }
    }
}
