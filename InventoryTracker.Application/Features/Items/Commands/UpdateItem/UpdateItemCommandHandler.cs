using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Items.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Items.Commands.UpdateItem
{
    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ItemDTO?>
    {
        private readonly IAppDbContext _context;
        public UpdateItemCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<ItemDTO?> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == request.ItemId, cancellationToken);
            if (item == null)
                return null;

            var skuExists = await _context.Items.AnyAsync(x => x.SKU == request.SKU && x.ItemId != request.ItemId, cancellationToken);
            if (skuExists)
            {
                throw new InvalidOperationException("Another item with the same SKU already exists.");
            }

            item.Name = request.Name;
            item.SKU = request.SKU;
            item.Description = request.Description;
            item.UnitOfMeasure = request.UnitOfMeasure;
            item.CreditValue = request.CreditValue;
            item.Weight = request.Weight;
            await _context.SaveChangesAsync(cancellationToken);
            return new ItemDTO
            {
                ItemId = item.ItemId,
                Name = item.Name,
                SKU = item.SKU,
                Description = item.Description,
                UnitOfMeasure = item.UnitOfMeasure,
                CreditValue = item.CreditValue,
                Weight = item.Weight,
                IsActive = item.IsActive,
                CreatedAt = item.CreatedAt,
                CreatedBy = item.CreatedBy ?? default!,
                UpdatedAt = item.UpdatedAt,
                UpdatedBy = item.UpdatedBy
            };
        }
    }
}
