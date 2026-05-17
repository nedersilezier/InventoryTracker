using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Countries.DTOs;
using InventoryTracker.Domain.Entities;
using MediatR;

namespace InventoryTracker.Application.Features.Countries.Commands.UpdateCountry
{
    public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, CountryDTO?>
    {
        private readonly IAppDbContext _context;
        private readonly ICountriesRepository _countriesRepository;
        public UpdateCountryCommandHandler(IAppDbContext context, ICountriesRepository countriesRepository)
        {
            _context = context;
            _countriesRepository = countriesRepository;
        }
        public async Task<CountryDTO?> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            request.Name = request.Name.Trim();
            var nameSplit = request.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < nameSplit.Length; i++)
            {
                nameSplit[i] = char.ToUpper(nameSplit[i][0]) + nameSplit[i].Substring(1).ToLower();
            }
            request.Name = string.Join(' ', nameSplit);
            request.Code = request.Code.Trim().ToUpper();

            var country = await _countriesRepository.GetCountryByIdAsync(request.CountryId, cancellationToken);
            if (country == null)
                throw new RecordNotFoundException(nameof(Country), request.CountryId);

            var codeExists = await _countriesRepository.CountryCodeExistsForUpdateAsync(request.Code, request.CountryId, cancellationToken);
            if (codeExists)
                throw new BusinessException($"Another country with code {request.Code} already exists.");
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