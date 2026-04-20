using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Stocks.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Stocks.Queries.GetStocksByWarehouse
{
    public class GetStocksQueryHandler: IRequestHandler<GetStocksQuery, PagedResult<StockDetailsDTO>>
    {
        private readonly IStocksQueryService  _stocksQueryService;
        public GetStocksQueryHandler(IStocksQueryService stocksQueryService)
        {
            _stocksQueryService = stocksQueryService;
        }
        public async Task<PagedResult<StockDetailsDTO>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
        {
            var parameters = new GetStocksParameters
            {
                WarehouseId = request.WarehouseId,
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                SearchTerm = request.SearchTerm,
            };
            return await _stocksQueryService.GetStocksAsync(parameters, cancellationToken);
        }
    }
}
