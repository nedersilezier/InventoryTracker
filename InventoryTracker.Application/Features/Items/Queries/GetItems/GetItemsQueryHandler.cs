using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Items.DTOs.Items;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Items.Queries.GetItems
{
    public class GetItemsQueryHandler: IRequestHandler<GetItemsQuery, List<ItemDTO>>
    {
        private readonly IAppDbContext _context;
        public GetItemsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ItemDTO>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Items
                .AsNoTracking()
                .Select(x => new ItemDTO
                {
                    ItemId = x.ItemId,
                    Name = x.Name,
                    SKU = x.SKU,
                    Description = x.Description,
                    UnitOfMeasure = x.UnitOfMeasure,
                    CreditValue = x.CreditValue,
                    Weight = x.Weight,
                    IsActive = x.IsActive,
                    //test
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy ?? default!,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy
                })
                .ToListAsync(cancellationToken);
        }
    }
}
