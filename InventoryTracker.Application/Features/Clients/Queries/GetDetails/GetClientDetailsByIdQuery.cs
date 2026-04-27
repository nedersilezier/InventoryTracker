using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Clients.Queries.GetDetails
{
    public class GetClientDetailsByIdQuery: IRequest<ClientDetailsDTO?>
    {
        public Guid ClientId { get; private set; }
        public GetClientDetailsByIdQuery(Guid clientId)
        {
            ClientId = clientId;
        }
    }
}
