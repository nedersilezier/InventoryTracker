using InventoryTracker.Application.Features.Stocks.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Stocks.Queries.GetStocksByWarehouse
{
    public class GetStocksByWarehouseIdQuery: IRequest<List<StockDetailsDTO>>
    {
        public Guid WarehouseId { get; set; }
        public GetStocksByWarehouseIdQuery(Guid warehouseId)
        {
            WarehouseId = warehouseId;
        }
    }
}
