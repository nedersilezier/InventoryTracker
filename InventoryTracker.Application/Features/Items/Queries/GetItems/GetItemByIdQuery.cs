using InventoryTracker.Application.Features.Items.DTOs.Items;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Items.Queries.GetItems
{
    public class GetItemByIdQuery: IRequest<ItemDTO>
    {
        public Guid ItemId { get; private set; }
        public GetItemByIdQuery(Guid id)
        {
            ItemId = id;
        }
    }
}
