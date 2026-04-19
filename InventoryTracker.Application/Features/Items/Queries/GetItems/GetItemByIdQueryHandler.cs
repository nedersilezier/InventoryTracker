using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Items.DTOs;
using InventoryTracker.Domain.Entities;
using MediatR;

namespace InventoryTracker.Application.Features.Items.Queries.GetItems
{
    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemDTO?>
    {
        private readonly IItemsQueryService _itemsQueryService;
        public GetItemByIdQueryHandler(IItemsQueryService itemsQueryService)
        {
            _itemsQueryService = itemsQueryService;
        }
        public async Task<ItemDTO?> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _itemsQueryService.GetItemByIdAsync(request.ItemId, cancellationToken);
            if (item == null)
            {
                throw new RecordNotFoundException(nameof(Item), request.ItemId);
            }
            return item;
        }
    }
}
