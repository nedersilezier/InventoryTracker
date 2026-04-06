using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Clients.Commands.UpdateClient
{
    public class UpdateClientCommandValidator: AbstractValidator<UpdateClientCommand>
    {
        public UpdateClientCommandValidator()
        {
            RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required.")
                    .MaximumLength(200);

            RuleFor(x => x.ClientCode)
                    .NotEmpty().WithMessage("Client code is required.")
                    .MaximumLength(50);

            RuleFor(x => x.Email)
                    .MaximumLength(150)
                    .WithMessage("E-mail address cannot exceed 150 characters")
                    .EmailAddress()
                    .WithMessage("Not a valid e-mail address.");

            RuleFor(x => x.PhoneNumber)
                    .MaximumLength(50)
                    .WithMessage("Phone number cannot exceed 50 characters.");

            //address related rules
            RuleFor(x => x.Address.Street)
                    .NotEmpty().WithMessage("Street is required.")
                    .MaximumLength(150).WithMessage("E-mail address cannot exceed 150 characters");

            RuleFor(x => x.Address.HouseNumber)
                    .NotEmpty().WithMessage("House number is required.")
                    .MaximumLength(20).WithMessage("House number cannot exceed 20 characters.");

            RuleFor(x => x.Address.ApartmentNumber)
                    .MaximumLength(20).WithMessage("Apartment number cannot exceed 20 characters.");

            RuleFor(x => x.Address.PostalCode)
                    .NotEmpty().WithMessage("Postal code is required.")
                    .MaximumLength(20).WithMessage("Postal code cannot exceed 20 characters.");

            RuleFor(x => x.Address.City)
                    .NotEmpty().WithMessage("City is required.")
                    .MaximumLength(150).WithMessage("City cannot exceed 150 characters.");
        }
    }
}
