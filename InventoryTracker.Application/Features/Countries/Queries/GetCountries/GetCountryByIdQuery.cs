using InventoryTracker.Application.Features.Countries.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Countries.Queries.GetCountries
{
    public class GetCountryByIdQuery: IRequest<CountryDTO?>
    {
        public Guid CountryId { get; private set; }
        public GetCountryByIdQuery(Guid countryId)
        {
            CountryId = countryId;
        }
    }
}
