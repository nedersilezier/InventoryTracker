using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Countries.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Countries.Queries.GetCountries
{
    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, List<CountryDTO>>
    {
        private readonly IAppDbContext _context;
        public GetCountriesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<CountryDTO>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Countries
                .AsNoTracking()
                .Select(c => new CountryDTO
                {
                    CountryId = c.CountryId,
                    Name = c.Name,
                    Code = c.Code,
                    CreatedBy = c.CreatedBy ?? string.Empty,
                    CreatedAt = c.CreatedAt,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedAt = c.UpdatedAt
                }).ToListAsync(cancellationToken);
        }
    }
}
