using InventoryTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface IItemsRepository
    {
        Task AddItem(Item item);
        Task<Item?> GetItemByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> SKUExistsAsync(string sku, CancellationToken cancellationToken, Guid? excludedId = null);
    }
}
