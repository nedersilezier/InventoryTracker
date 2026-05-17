using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Countries.DTOs;
using InventoryTracker.Domain.Entities;
using MediatR;

namespace InventoryTracker.Application.Features.Countries.Commands.CreateCountry
{
    public class CreateCountryCommandHandler: IRequestHandler<CreateCountryCommand, CountryCreatedDTO>
    {
        private readonly IAppDbContext _context;
        private readonly ICountriesRepository _countriesRepository;
        public CreateCountryCommandHandler(IAppDbContext context, ICountriesRepository countriesRepository)
        {
            _context = context;
            _countriesRepository = countriesRepository;
        }
        public async Task<CountryCreatedDTO> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            request.Name = request.Name.Trim();
            var nameSplit = request.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < nameSplit.Length; i++)
            {
                nameSplit[i] = char.ToUpper(nameSplit[i][0]) + nameSplit[i].Substring(1).ToLower();
            }
            request.Name = string.Join(' ', nameSplit);
            request.Code = request.Code.Trim().ToUpper();
            var countryCodeExists = await _countriesRepository.CountryCodeExistsAsync(request.Code, cancellationToken);
            if (countryCodeExists)
                throw new BusinessException($"Country with code {request.Code} already exists.");
            var country = new Country
            {
                CountryId = Guid.NewGuid(),
                Name = request.Name,
                Code = request.Code
            };

            await _countriesRepository.AddCountry(country, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new CountryCreatedDTO
            {
                CountryId = country.CountryId,
                Name = country.Name,
            };
        }
    }
}
