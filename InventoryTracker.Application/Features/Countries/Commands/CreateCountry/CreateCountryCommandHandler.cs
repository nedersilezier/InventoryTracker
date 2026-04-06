using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Countries.DTOs;
using InventoryTracker.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Application.Features.Countries.Commands.CreateCountry
{
    public class CreateCountryCommandHandler: IRequestHandler<CreateCountryCommand, CountryDTO>
    {
        private readonly IAppDbContext _context;
        public CreateCountryCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<CountryDTO> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            var countryCodeExists = await _context.Countries.AnyAsync(c => c.Code == request.Code, cancellationToken);
            if (countryCodeExists)
                throw new Exception($"Country with code {request.Code} already exists.");

            var country = new Country
            {
                CountryId = Guid.NewGuid(),
                Name = request.Name,
                Code = request.Code
            };

            _context.Countries.Add(country);
            await _context.SaveChangesAsync(cancellationToken);
            return new CountryDTO
            {
                CountryId = country.CountryId,
                Name = country.Name,
                Code = country.Code
            };
        }
    }
}
