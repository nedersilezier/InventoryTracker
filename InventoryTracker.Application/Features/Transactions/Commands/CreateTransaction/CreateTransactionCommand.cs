using InventoryTracker.Application.Features.Transactions.DTOs;
using InventoryTracker.Shared.Enums;
using MediatR;

namespace InventoryTracker.Application.Features.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommand : IRequest<Guid>
    {
        public TransactionType Type { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? SourceWarehouseId { get; set; }
        public Guid? DestinationWarehouseId { get; set; }
        public DateTime TransactionDate { get; set;  }
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public List<CreateTransactionItemDTO> Items { get; set; } = new();

        public class CreateTransactionItemDTO
        {
            public Guid ItemId { get; set; }
            public decimal Quantity { get; set; }
        }
    }
}
