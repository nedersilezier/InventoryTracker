using InventoryTracker.Application.Features.Countries.DTOs;
using MediatR;

using InventoryTracker.Application.Common.Interfaces;

namespace InventoryTracker.Application.Features.Countries.Queries.GetCountries
{
    public class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, CountryDTO?>
    {
        private readonly ICountriesQueryService _countriesService;
        public GetCountryByIdQueryHandler(ICountriesQueryService countriesService)
        {
            _countriesService = countriesService;
        }
        public async Task<CountryDTO?> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            var country = await _countriesService.GetCountryByIdAsync(request.CountryId, cancellationToken);
            return country;
        }
    }
}
