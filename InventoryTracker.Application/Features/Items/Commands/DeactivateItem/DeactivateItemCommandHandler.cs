using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Items.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Items.Commands.DeactivateItem
{
    public class DeactivateItemCommandHandler : IRequestHandler<DeactivateItemCommand, bool>
    {
        private readonly IAppDbContext _context;
        public DeactivateItemCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(DeactivateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == request.ItemId);
            if (item == null)
                return false;
            item.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
