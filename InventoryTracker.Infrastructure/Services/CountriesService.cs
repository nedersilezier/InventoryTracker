using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Countries.Commands.CreateCountry;
using InventoryTracker.Application.Features.Countries.Commands.UpdateCountry;
using InventoryTracker.Application.Features.Countries.DTOs;
using InventoryTracker.Application.Features.Countries.Queries.GetCountries;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Infrastructure.Services
{
    public class CountriesService : ICountriesService
    {
        private readonly AppDbContext _context;
        public CountriesService(AppDbContext context)
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
        public async Task<CountryDTO> CreateCountryAsync(CreateCountryParameters parameters, CancellationToken cancellationToken)
        {
            var countryCodeExists = await _context.Countries.AnyAsync(c => c.Code == parameters.Code, cancellationToken);
            if (countryCodeExists)
                throw new BusinessException($"Country with code {parameters.Code} already exists.");
            var country = new Country
            {
                CountryId = Guid.NewGuid(),
                Name = parameters.Name,
                Code = parameters.Code
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
        public async Task<CountryDTO?> UpdateCountryAsync(UpdateCountryParameters parameters, CancellationToken cancellationToken)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.CountryId == parameters.CountryId, cancellationToken);
            if (country == null)
                return null;

            var codeExists = await _context.Countries.AnyAsync(x => x.Code == parameters.Code && x.CountryId != parameters.CountryId, cancellationToken);
            if (codeExists)
                throw new BusinessException($"Another country with code {parameters.Code} already exists.");

            country.Name = parameters.Name;
            country.Code = parameters.Code;
            await _context.SaveChangesAsync(cancellationToken);
            return new CountryDTO
            {
                CountryId = country.CountryId,
                Name = country.Name,
                Code = country.Code
            };
        }
        public async Task DeleteCountryAsync(Guid id, CancellationToken cancellationToken)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.CountryId == id, cancellationToken);
            if (country == null)
                return;
            var addressesDepend = await _context.Addresses.AnyAsync(a => a.CountryId == id, cancellationToken);
            if (addressesDepend)
                throw new ConflictException($"Country with name {country.Name} is used by addresses and cannot be deleted.");
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
