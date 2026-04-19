using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Items.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface IItemsQueryService
    {
        Task<PagedResult<ItemDTO>> GetAllItemsAsync(GetItemsParameters parameters, CancellationToken cancellationToken);
        Task<ItemDTO?> GetItemByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
