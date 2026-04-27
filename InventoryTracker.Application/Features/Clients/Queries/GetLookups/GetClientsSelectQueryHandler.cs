using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Clients.Queries.GetLookups
{
    public class GetClientsSelectQueryHandler: IRequestHandler<GetClientsSelectQuery, IReadOnlyList<InternalClientSelectDTO>>
    {
        private readonly IClientsQueryService _clientsQueryService;
        public GetClientsSelectQueryHandler(IClientsQueryService clientsQueryService)
        {
            _clientsQueryService = clientsQueryService;
        }
        public async Task<IReadOnlyList<InternalClientSelectDTO>> Handle(GetClientsSelectQuery query, CancellationToken cancellationToken)
        {
            return await _clientsQueryService.GetAllClientsLookupAsync(cancellationToken);
        }
    }
}
