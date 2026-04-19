using FluentValidation;

namespace InventoryTracker.Application.Features.Warehouses.Commands.UpdateWarehouse
{
    public class UpdateWarehouseCommandValidator: AbstractValidator<UpdateWarehouseCommand>
    {
        public UpdateWarehouseCommandValidator()
        {
            RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required.")
                    .MaximumLength(150).WithMessage("Name cannot exceed 150 characters.");

            RuleFor(x => x.Code)
                    .NotEmpty().WithMessage("Warehouse code is required.")
                    .MaximumLength(50).WithMessage("Warehouse code cannot exceed 50 characters");

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
