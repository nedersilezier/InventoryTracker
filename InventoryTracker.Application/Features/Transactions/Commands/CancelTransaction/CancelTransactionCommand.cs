using InventoryTracker.Application.Features.Transactions.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Transactions.Commands.CancelTransaction
{
    public class CancelTransactionCommand: IRequest<TransactionDTO?>
    {
        public Guid TransactionId {  get; set; }
        public string CancellationReason { get; set; } = string.Empty;
    }
}
