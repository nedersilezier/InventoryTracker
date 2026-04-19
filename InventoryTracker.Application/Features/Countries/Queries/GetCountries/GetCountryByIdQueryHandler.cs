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
        private readonly ICountriesService _countriesService;
        public GetCountryByIdQueryHandler(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }
        public async Task<CountryDTO?> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            var country = await _countriesService.GetCountryById(request.CountryId, cancellationToken);
            return country;
        }
    }
}
