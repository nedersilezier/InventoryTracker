using InventoryTracker.Application.Features.Clients.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Clients.Queries.GetClients
{
    public class GetClientsQuery: IRequest<List<ClientDTO>>
    {
    }
}
