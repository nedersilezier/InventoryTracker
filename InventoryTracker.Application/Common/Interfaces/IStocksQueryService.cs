using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Stocks.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface IStocksQueryService
    {
        Task<PagedResult<StockDetailsDTO>> GetStocksAsync(GetStocksParameters parameters, CancellationToken cancellationToken);
    }
}
