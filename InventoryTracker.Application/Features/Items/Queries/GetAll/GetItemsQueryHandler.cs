using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Items.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Items.Queries.GetItems
{
    public class GetItemsQueryHandler: IRequestHandler<GetItemsQuery, PagedResult<ItemDTO>>
    {
        private readonly IItemsQueryService _itemsQueryService;
        public GetItemsQueryHandler(IItemsQueryService itemsQueryService)
        {
            _itemsQueryService = itemsQueryService;
        }
        public async Task<PagedResult<ItemDTO>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            request.PageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            var parameters = new GetItemsParameters
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                SearchTerm = request.SearchTerm
            };
            return await _itemsQueryService.GetAllItemsAsync(parameters, cancellationToken);
        }
    }
}
