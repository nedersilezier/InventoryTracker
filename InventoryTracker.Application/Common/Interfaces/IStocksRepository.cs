using InventoryTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface IStocksRepository
    {
        Task<IReadOnlyList<Stock>> GetStocksByItemIdsAsync(IEnumerable<Guid> itemIds, CancellationToken cancellationToken);
        public Task AddStock(Stock stock);
    }
}
