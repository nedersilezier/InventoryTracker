using InventoryTracker.Application.Features.Items.DTOs;
using InventoryTracker.Application.Features.Items.Queries.GetItems;
using MediatR;

namespace InventoryTracker.Application.Features.Items.Commands.DeactivateItem
{
    public class DeactivateItemCommand: IRequest<ItemDTO?>
    {
        public Guid ItemId { get; private set; }
        public DeactivateItemCommand(Guid itemId)
        {
            ItemId = itemId;
        }
    }
}
