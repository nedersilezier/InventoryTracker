using InventoryTracker.Application.Common.Interfaces;
using MediatR;
using InventoryTracker.Application.Features.Items.DTOs;

namespace InventoryTracker.Application.Features.Items.Commands.ActivateItem
{
    public class ActivateItemCommandHandler : IRequestHandler<ActivateItemCommand, ItemDTO?>
    {
        private readonly IAppDbContext _context;
        private readonly IItemsRepository _itemsRepository;
        public ActivateItemCommandHandler(IAppDbContext context, IItemsRepository itemsRepository)
        {
            _context = context;
            _itemsRepository = itemsRepository;
        }
        public async Task<ItemDTO?> Handle(ActivateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemsRepository.GetItemByIdAsync(request.ItemId, cancellationToken);
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
