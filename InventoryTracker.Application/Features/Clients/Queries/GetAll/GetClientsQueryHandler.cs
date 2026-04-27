using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Clients.Queries.GetClients
{
    public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, PagedResult<ClientDTO>>
    {
        private readonly IClientsQueryService _clientsService;
        public GetClientsQueryHandler(IClientsQueryService clientsService)
        {
            _clientsService = clientsService;
        }
        public async Task<PagedResult<ClientDTO>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
        {
            request.PageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            var parameters = new GetClientsParameters
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                SearchTerm = request.SearchTerm
            };
            return await _clientsService.GetAllClientsAsync(parameters, cancellationToken);
        }
    }
}
