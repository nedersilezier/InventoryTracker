using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Items.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Items.Queries.GetItems
{
    public class GetItemsQuery: IRequest<PagedResult<ItemDTO>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; } = 1;
        public string? SearchTerm { get; set; }
    }
}
