using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Countries.DTOs;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Infrastructure.Services
{
    public class CountriesQueryService : ICountriesQueryService
    {
        private readonly AppDbContext _context;
        public CountriesQueryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<CountryDTO>> GetAllCountriesAsync(GetCountriesParameters parameters, CancellationToken cancellationToken)
        {
            var query = _context.Countries.AsQueryable();
            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(c => c.Name.Contains(parameters.SearchTerm)
                                    || c.Code.Contains(parameters.SearchTerm));
            }
            var totalCount = await query.CountAsync(cancellationToken);
            var totalPages = totalCount == 0 ? 1 : (int)Math.Ceiling((double)totalCount / parameters.PageSize);
            var pageNumber = parameters.PageNumber;
            if (pageNumber > totalPages)
                pageNumber = totalPages;
            var countries = await query.OrderBy(c => c.Name).Skip((pageNumber - 1) * parameters.PageSize).Take(parameters.PageSize).ToListAsync(cancellationToken);
            var countriesDTO = new List<CountryDTO>();
            foreach (var country in countries)
            {
                countriesDTO.Add(new CountryDTO
                {
                    CountryId = country.CountryId,
                    Name = country.Name,
                    Code = country.Code,
                    CreatedBy = country.CreatedBy ?? string.Empty,
                    CreatedAt = country.CreatedAt,
                    UpdatedBy = country.UpdatedBy,
                    UpdatedAt = country.UpdatedAt
                });
            }
            return new PagedResult<CountryDTO>
            {
                Items = countriesDTO,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = parameters.PageSize,
                TotalCount = totalCount
            };
        }
        public async Task<IReadOnlyList<InternalCountrySelectDTO>> GetAllCountriesLookupAsync(CancellationToken cancellationToken)
        {
            var countries = await _context.Countries.AsNoTracking().OrderBy(w => w.Name).Select(w => new InternalCountrySelectDTO
            {
                CountryId = w.CountryId,
                CountryName = w.Name
            }).ToListAsync(cancellationToken);
            return countries;
        }
        public async Task<CountryDTO?> GetCountryByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var country = await _context.Countries
                .AsNoTracking()
                .Where(x => x.CountryId == id)
                .Select(x => new CountryDTO
                {
                    CountryId = x.CountryId,
                    Name = x.Name,
                    Code = x.Code
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (country == null)
                throw new RecordNotFoundException(nameof(Country), id);
            return country;
        }
    }
}
