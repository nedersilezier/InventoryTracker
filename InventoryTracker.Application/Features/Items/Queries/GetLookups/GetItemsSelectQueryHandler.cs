using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Items.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Items.Queries.GetLookups
{
    public class GetItemsSelectQueryHandler: IRequestHandler<GetItemsSelectQuery, IReadOnlyList<InternalItemSelectDTO>>
    {
        private readonly IItemsQueryService _itemsQueryService;
        public GetItemsSelectQueryHandler(IItemsQueryService itemsQueryService)
        {
            _itemsQueryService = itemsQueryService;
        }
        public async Task<IReadOnlyList<InternalItemSelectDTO>> Handle(GetItemsSelectQuery query, CancellationToken cancellationToken)
        {
            return await _itemsQueryService.GetAllItemsLookupAsync(cancellationToken);
        }
    }
}
