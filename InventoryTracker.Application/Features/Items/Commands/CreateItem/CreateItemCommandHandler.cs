using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Items.DTOs.Items;
using InventoryTracker.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Items.Commands.CreateItem
{
    public class CreateItemCommandHandler: IRequestHandler<CreateItemCommand, ItemDTO>
    {
        private readonly IAppDbContext _context;
        public CreateItemCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<ItemDTO> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var skuExists = await _context.Items.AnyAsync(i => i.SKU == request.SKU, cancellationToken);
            if(skuExists)
            {
                throw new InvalidOperationException($"An item with SKU '{request.SKU}' already exists.");
            }

            var item = new Item
            {
                ItemId = Guid.NewGuid(),
                Name = request.Name,
                SKU = request.SKU,
                Description = request.Description,
                UnitOfMeasure = request.UnitOfMeasure,
                CreditValue = request.CreditValue,
                Weight = request.Weight,
                IsActive = true
            };
            _context.Items.Add(item);
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
                IsActive = item.IsActive
            };
        }
    }
}
