using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Items.DTOs;
using MediatR;

namespace InventoryTracker.Application.Features.Items.Commands.DeactivateItem
{
    public class DeactivateItemCommandHandler : IRequestHandler<DeactivateItemCommand, ItemDTO?>
    {
        private readonly IAppDbContext _context;
        private readonly IItemsRepository _itemsRepository;
        public DeactivateItemCommandHandler(IAppDbContext context, IItemsRepository itemsRepository)
        {
            _context = context;
            _itemsRepository = itemsRepository;
        }
        public async Task<ItemDTO?> Handle(DeactivateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemsRepository.GetItemByIdAsync(request.ItemId, cancellationToken);
            if (item == null)
                return null;
            item.IsActive = false;
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
