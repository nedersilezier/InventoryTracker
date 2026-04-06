using InventoryTracker.Application.Features.Countries.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using InventoryTracker.Application.Common.Interfaces;

namespace InventoryTracker.Application.Features.Countries.Queries.GetCountries
{
    public class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, CountryDTO?>
    {
        private readonly IAppDbContext _context;
        public GetCountryByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<CountryDTO?> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            var country = await _context.Countries
                .AsNoTracking()
                .Where(x => x.CountryId == request.CountryId)
                .Select(x => new CountryDTO
                {
                    CountryId = x.CountryId,
                    Name = x.Name,
                    Code = x.Code
                })
                .FirstOrDefaultAsync(cancellationToken);
            return country;
        }
    }
}
