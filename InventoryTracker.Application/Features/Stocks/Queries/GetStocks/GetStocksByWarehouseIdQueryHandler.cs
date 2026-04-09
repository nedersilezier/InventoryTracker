using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Stocks.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Stocks.Queries.GetStocksByWarehouse
{
    public class GetStocksByWarehouseIdQueryHandler: IRequestHandler<GetStocksByWarehouseIdQuery, List<StockDetailsDTO>>
    {
        private readonly IAppDbContext _context;
        public GetStocksByWarehouseIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<StockDetailsDTO>> Handle(GetStocksByWarehouseIdQuery request, CancellationToken cancellationToken)
        {
            var stocks = await _context.Stocks
                .Include(s => s.Item)
                .AsNoTracking()
                .Where(s => s.WarehouseId == request.WarehouseId)
                .Select(s => new StockDetailsDTO
                {
                    StockId = s.StockId,
                    WarehouseId = s.WarehouseId,
                    ItemId = s.ItemId,
                    ItemName = s.Item.Name,
                    SKU = s.Item.SKU,
                    UnitOfMeasure = s.Item.UnitOfMeasure,
                    CreditValue = s.Item.CreditValue,
                    Weight = s.Item.Weight,
                    Quantity = s.Quantity,
                    CreatedBy = s.CreatedBy,
                    CreatedAt = s.CreatedAt,
                    UpdatedBy = s.UpdatedBy,
                    UpdatedAt = s.UpdatedAt
                })
                .ToListAsync(cancellationToken);
            return stocks;
        }
    }
}
