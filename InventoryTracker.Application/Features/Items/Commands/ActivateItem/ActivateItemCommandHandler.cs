using InventoryTracker.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using InventoryTracker.Application.Features.Items.DTOs;

namespace InventoryTracker.Application.Features.Items.Commands.ActivateItem
{
    public class ActivateItemCommandHandler : IRequestHandler<ActivateItemCommand, ItemDTO?>
    {
        private readonly IAppDbContext _context;
        public ActivateItemCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<ItemDTO?> Handle(ActivateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == request.ItemId, cancellationToken);
            if (item == null)
                return null;
            item.IsActive = true;
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
