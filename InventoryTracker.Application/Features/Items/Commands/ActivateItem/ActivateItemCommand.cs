using InventoryTracker.Application.Features.Items.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Items.Commands.ActivateItem
{
    public class ActivateItemCommand: IRequest<ItemDTO?>
    {
        public Guid ItemId { get; private set; }
        public ActivateItemCommand(Guid itemId)
        {
            ItemId = itemId;
        }
    }
}
