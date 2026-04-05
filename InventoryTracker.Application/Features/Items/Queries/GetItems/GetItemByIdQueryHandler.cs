using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Items.DTOs.Items;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Items.Queries.GetItems
{
    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemDTO>
    {
        private readonly IAppDbContext _context;
        public GetItemByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<ItemDTO?> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .AsNoTracking()
                .Where(x => x.ItemId == request.ItemId)
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
                    CreatedBy = x.CreatedBy ?? string.Empty,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy,
                })
                .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }
}
