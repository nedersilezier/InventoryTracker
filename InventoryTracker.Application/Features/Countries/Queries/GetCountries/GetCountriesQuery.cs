using InventoryTracker.Application.Features.Countries.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Countries.Queries.GetCountries
{
    public class GetCountriesQuery: IRequest<List<CountryDTO>>
    {
    }
}
