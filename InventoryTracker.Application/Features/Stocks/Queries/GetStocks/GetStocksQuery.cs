using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Stocks.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Stocks.Queries.GetStocksByWarehouse
{
    public class GetStocksQuery: IRequest<PagedResult<StockDetailsDTO>>
    {
        public Guid? WarehouseId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; } = 1;
        public string? SearchTerm { get; set; }
    }
}
