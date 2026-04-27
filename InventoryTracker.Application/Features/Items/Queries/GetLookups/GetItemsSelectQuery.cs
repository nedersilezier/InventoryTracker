using InventoryTracker.Application.Features.Items.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Items.Queries.GetLookups
{
    public class GetItemsSelectQuery: IRequest<IReadOnlyList<InternalItemSelectDTO>>
    {
    }
}
