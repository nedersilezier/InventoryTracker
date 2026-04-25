using InventoryTracker.Application.Features.Countries.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Countries.Queries.GetCountries
{
    public class GetCountriesSelectQuery: IRequest<IReadOnlyList<InternalCountrySelectDTO>>
    {
    }
}
