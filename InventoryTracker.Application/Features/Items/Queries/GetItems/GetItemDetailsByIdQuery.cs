using InventoryTracker.Application.Features.Items.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Items.Queries.GetItems
{
    public class GetItemDetailsByIdQuery: IRequest<ItemDetailsDTO?>
    {
        public Guid ItemId { get; private set; }
        public GetItemDetailsByIdQuery(Guid id)
        {
            ItemId = id;
        }
    }
}
