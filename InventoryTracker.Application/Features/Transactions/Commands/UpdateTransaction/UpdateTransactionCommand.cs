using InventoryTracker.Application.Features.Transactions.DTOs;
using InventoryTracker.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionCommand: IRequest<TransactionDTO?>
    {
        public Guid TransactionId { get; set; }
        public TransactionType Type { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? SourceWarehouseId { get; set; }
        public Guid? DestinationWarehouseId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public List<UpdateTransactionItemDTO> Items { get; set; } = new();

        public class UpdateTransactionItemDTO
        {
            public Guid ItemId { get; set; }
            public decimal Quantity { get; set; }
        }
    }
}
