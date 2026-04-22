using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Clients.DTOs;
using InventoryTracker.Application.Features.Items.DTOs;
using InventoryTracker.Application.Features.Items.Queries.GetItems;
using InventoryTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Infrastructure.Services
{
    public class ItemsQueryService: IItemsQueryService
    {
        private readonly AppDbContext _context;
        public ItemsQueryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<ItemDTO>> GetAllItemsAsync(GetItemsParameters parameters, CancellationToken cancellationToken)
        {
            var query = _context.Items.AsQueryable();
            var totalActive = await query.CountAsync(i => i.IsActive, cancellationToken);
            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(i => i.Name.Contains(parameters.SearchTerm)
                                    || i.SKU.Contains(parameters.SearchTerm)
                                    || (i.Description != null && i.Description.Contains(parameters.SearchTerm))
                                    || i.UnitOfMeasure.Contains(parameters.SearchTerm));
            }
            var totalCount = await query.CountAsync(cancellationToken);
            var totalPages = totalCount == 0 ? 1 : (int)Math.Ceiling((double)totalCount / parameters.PageSize);
            var pageNumber = parameters.PageNumber;
            if (pageNumber > totalPages)
                pageNumber = totalPages;

            var items = await query.OrderBy(i => i.Name).Skip((pageNumber - 1) * parameters.PageSize).Take(parameters.PageSize).ToListAsync(cancellationToken);
            var itemsDTO = new List<ItemDTO>();
            foreach (var item in items)
            {
                itemsDTO.Add(new ItemDTO
                {
                    ItemId = item.ItemId,
                    Name = item.Name,
                    SKU = item.SKU,
                    Description = item.Description,
                    UnitOfMeasure = item.UnitOfMeasure,
                    CreditValue = item.CreditValue,
                    Weight = item.Weight,
                    IsActive = item.IsActive
                });

            }

            return new PagedResult<ItemDTO>
            {
                Items = itemsDTO,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = parameters.PageSize,
                TotalCount = totalCount,
                TotalActive = totalActive,
            };
        }
        public async Task<ItemDTO?> GetItemByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .AsNoTracking()
                .Where(i => i.ItemId == id)
                .Select(i => new ItemDTO
                {
                    ItemId = i.ItemId,
                    Name = i.Name,
                    SKU = i.SKU,
                    Description = i.Description,
                    UnitOfMeasure = i.UnitOfMeasure,
                    CreditValue = i.CreditValue,
                    Weight = i.Weight,
                    IsActive = i.IsActive,
                    //test
                    CreatedAt = i.CreatedAt,
                    CreatedBy = i.CreatedBy ?? string.Empty,
                    UpdatedAt = i.UpdatedAt,
                    UpdatedBy = i.UpdatedBy,
                })
                .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }
}
