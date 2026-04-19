using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Countries.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Countries.Queries.GetCountries
{
    public class GetCountriesQuery: IRequest<PagedResult<CountryDTO>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; } = 1;
        public string? SearchTerm { get; set; }
    }
}
