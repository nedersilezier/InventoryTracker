using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Clients.DTOs;
using InventoryTracker.Domain.Entities;
using MediatR;

namespace InventoryTracker.Application.Features.Clients.Queries.GetClients
{
    public class GetClientDetailsByIdQueryHandler: IRequestHandler<GetClientDetailsByIdQuery, ClientDetailsDTO?>
    {
        private readonly IClientsQueryService _clientsQueryService;
        public GetClientDetailsByIdQueryHandler(IClientsQueryService clientsQueryService)
        {             
            _clientsQueryService = clientsQueryService;
        }

        public async Task<ClientDetailsDTO?> Handle(GetClientDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var client = await _clientsQueryService.GetClientDetailsByIdAsync(request.ClientId, cancellationToken);
            if (client == null)
                throw new RecordNotFoundException(nameof(Client), request.ClientId);
            return client;
        }
    }
}
