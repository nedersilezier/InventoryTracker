using InventoryTracker.Application.Features.Countries.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using InventoryTracker.Application.Common.Interfaces;

namespace InventoryTracker.Application.Features.Countries.Commands.UpdateCountry
{
    public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, CountryDTO?>
    {
        private readonly IAppDbContext _context;
        public UpdateCountryCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<CountryDTO?> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.CountryId == request.CountryId, cancellationToken);
            if (country == null)
                return null;

            var codeExists = await _context.Countries.AnyAsync(x => x.Code == request.Code && x.CountryId != request.CountryId, cancellationToken);
            if (codeExists)
                throw new InvalidOperationException($"Another country with code {request.Code} already exists.");

            country.Name = request.Name;
            country.Code = request.Code;
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