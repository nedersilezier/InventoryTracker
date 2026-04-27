using InventoryTracker.Application.Features.Items.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Items.Queries.GetById
{
    public class GetItemByIdQuery: IRequest<ItemDTO?>
    {
        public Guid ItemId { get; private set; }
        public GetItemByIdQuery(Guid id)
        {
            ItemId = id;
        }
    }
}
