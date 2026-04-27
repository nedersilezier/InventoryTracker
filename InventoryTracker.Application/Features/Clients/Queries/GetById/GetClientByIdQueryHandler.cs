using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Clients.Queries.GetById
{
    public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, ClientDTO?>
    {
        private readonly IClientsQueryService _clientsService;
        public GetClientByIdQueryHandler(IClientsQueryService clientsService)
        {
            _clientsService = clientsService;
        }
        public async Task<ClientDTO?> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
        {
            return await _clientsService.GetClientByIdAsync(request.ClientId, cancellationToken);
        }
    }
}
