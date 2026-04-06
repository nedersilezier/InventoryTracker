using InventoryTracker.Application.Features.Items.DTOs;
using InventoryTracker.Application.Features.Items.Queries.GetItems;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Items.Commands.DeactivateItem
{
    public class DeactivateItemCommand: IRequest<bool>
    {
        public Guid ItemId { get; private set; }
        public DeactivateItemCommand(Guid itemId)
        {
            ItemId = itemId;
        }
    }
}
