using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Items.DTOs;
using InventoryTracker.Domain.Entities;
using MediatR;

namespace InventoryTracker.Application.Features.Items.Queries.GetItems
{
    public class GetItemDetailsByIdQueryHandler : IRequestHandler<GetItemDetailsByIdQuery, ItemDetailsDTO?>
    {
        private readonly IItemsQueryService _itemsQueryService;
        public GetItemDetailsByIdQueryHandler(IItemsQueryService itemsQueryService)
        {
            _itemsQueryService = itemsQueryService;
        }
        public async Task<ItemDetailsDTO?> Handle(GetItemDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _itemsQueryService.GetItemDetailsByIdAsync(request.ItemId, cancellationToken);
            if (item == null)
            {
                throw new RecordNotFoundException(nameof(Item), request.ItemId);
            }
            return item;
        }
    }
}
