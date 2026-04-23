using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Items.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Items.Commands.UpdateItem
{
    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ItemDTO?>
    {
        private readonly IAppDbContext _context;
        private readonly IItemsRepository _itemsRepository;
        public UpdateItemCommandHandler(IAppDbContext context, IItemsRepository itemsRepository)
        {
            _context = context;
            _itemsRepository = itemsRepository;
        }
        public async Task<ItemDTO?> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemsRepository.GetItemByIdAsync(request.ItemId, cancellationToken);
            if (item == null)
                return null;

            var skuExists = await _itemsRepository.SKUExistsAsync(request.SKU, cancellationToken, request.ItemId);
            if (skuExists)
            {
                throw new BusinessException("Another item with the same SKU already exists.");
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
                IsActive = item.IsActive
            };
        }
    }
}
