using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Countries.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Countries.Queries.GetCountries
{
    public class GetCountriesSelectQueryHandler : IRequestHandler<GetCountriesSelectQuery, IReadOnlyList<InternalCountrySelectDTO>>
    {
        private readonly ICountriesService _countriesService;
        public GetCountriesSelectQueryHandler(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }
        public async Task<IReadOnlyList<InternalCountrySelectDTO>> Handle(GetCountriesSelectQuery request, CancellationToken cancellationToken)
        {
            return await _countriesService.GetAllCountriesLookupAsync(cancellationToken);
        }
    }
}
