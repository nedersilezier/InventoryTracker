using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Countries.DTOs;
using InventoryTracker.Application.Features.Users.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Countries.Queries.GetCountries
{
    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, PagedResult<CountryDTO>>
    {
        private readonly ICountriesService _countriesService;
        public GetCountriesQueryHandler(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }
        public async Task<PagedResult<CountryDTO>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            request.PageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            var parameters = new GetCountriesParameters
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                SearchTerm = request.SearchTerm
            };
            return await _countriesService.GetAllCountriesAsync(parameters, cancellationToken);
        }
    }
}
