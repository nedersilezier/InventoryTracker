using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Stocks.DTOs;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Infrastructure.Services
{
    public class StocksQueryService : IStocksQueryService
    {
        private readonly AppDbContext _context;
        public StocksQueryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<StockDetailsDTO>> GetStocksAsync(GetStocksParameters parameters, CancellationToken cancellationToken)
        {
            var query = _context.Stocks.Include(s => s.Warehouse).Include(s => s.Item).AsQueryable();
            if (parameters.WarehouseId.HasValue)
            {
                var warehouseExists = await _context.Warehouses.AnyAsync(w => w.WarehouseId == parameters.WarehouseId.Value, cancellationToken);
                if (!warehouseExists)
                    throw new RecordNotFoundException(nameof(Warehouse), parameters.WarehouseId);
                query = query.Where(s => s.WarehouseId == parameters.WarehouseId.Value);
            }
            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(s => s.Item.Name.Contains(parameters.SearchTerm)
                                    || s.Item.SKU.Contains(parameters.SearchTerm)
                                    || (s.Item.Description != null && s.Item.Description.Contains(parameters.SearchTerm))
                                    || s.Item.UnitOfMeasure.Contains(parameters.SearchTerm)
                                    || s.Warehouse.Name.Contains(parameters.SearchTerm));
            }
            var totalCount = await query.CountAsync(cancellationToken);
            var totalPages = totalCount == 0 ? 1 : (int)Math.Ceiling((double)totalCount / parameters.PageSize);
            var pageNumber = parameters.PageNumber;
            if (pageNumber > totalPages)
                pageNumber = totalPages;

            var stocks = await query.OrderBy(s => s.Item.Name).Skip((pageNumber - 1) * parameters.PageSize).Take(parameters.PageSize).ToListAsync(cancellationToken);
            var stocksDTO = new List<StockDetailsDTO>();
            foreach (var stock in stocks)
            {
                stocksDTO.Add(new StockDetailsDTO
                {
                    StockId = stock.StockId,
                    WarehouseId = stock.WarehouseId,
                    WarehouseName = stock.Warehouse.Name,
                    ItemId = stock.ItemId,
                    ItemName = stock.Item.Name,
                    SKU = stock.Item.SKU,
                    UnitOfMeasure = stock.Item.UnitOfMeasure,
                    CreditValue = stock.Item.CreditValue,
                    Weight = stock.Item.Weight,
                    Quantity = stock.Quantity,
                });

            }

            return new PagedResult<StockDetailsDTO>
            {
                Items = stocksDTO,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = parameters.PageSize,
                TotalCount = totalCount
            };
        }
    }
}
