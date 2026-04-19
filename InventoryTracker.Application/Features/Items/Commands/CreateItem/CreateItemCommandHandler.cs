using InventoryTracker.Application.Common.Exceptions;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Items.DTOs;
using InventoryTracker.Domain.Entities;
using MediatR;

namespace InventoryTracker.Application.Features.Items.Commands.CreateItem
{
    public class CreateItemCommandHandler: IRequestHandler<CreateItemCommand, ItemDTO>
    {
        private readonly IAppDbContext _context;
        private readonly IItemsRepository _itemsRepository;
        public CreateItemCommandHandler(IAppDbContext context, IItemsRepository itemsRepository)
        {
            _context = context;
            _itemsRepository = itemsRepository;
        }
        public async Task<ItemDTO> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var skuExists = await _itemsRepository.SKUExistsAsync(request.SKU, cancellationToken);
            if(skuExists)
            {
                throw new BusinessException($"An item with SKU '{request.SKU}' already exists.");
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
            await _itemsRepository.AddItem(item);
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
