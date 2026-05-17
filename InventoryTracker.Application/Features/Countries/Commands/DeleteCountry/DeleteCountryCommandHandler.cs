using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Domain.Entities;
using MediatR;


namespace InventoryTracker.Application.Features.Countries.Commands.DeleteCountry
{
    public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly ICountriesRepository _countriesRepository;
        private readonly IAddressesRepository _addressesRepository;
        public DeleteCountryCommandHandler(IAppDbContext context, ICountriesRepository countriesRepository, IAddressesRepository addressesRepository)
        {
            _context = context;
            _countriesRepository = countriesRepository;
            _addressesRepository = addressesRepository;
        }
        public async Task<Unit> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            var country = await _countriesRepository.GetCountryByIdAsync(request.CountryId, cancellationToken);
            if (country == null)
                throw new RecordNotFoundException(nameof(Country), request.CountryId);

            var addressesDepend = await _addressesRepository.HasAnyForCountryAsync(request.CountryId, cancellationToken);
            if (addressesDepend)
                throw new ConflictException($"Country with name {country.Name} is used by addresses and cannot be deleted.");
            await _countriesRepository.DeleteCountry(country, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}