using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Clients.Queries.GetLookups
{
    public class GetClientsSelectQuery: IRequest<IReadOnlyList<InternalClientSelectDTO>>
    {
    }
}
