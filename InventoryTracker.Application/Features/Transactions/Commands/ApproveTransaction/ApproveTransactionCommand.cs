using InventoryTracker.Application.Features.Transactions.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Transactions.Commands.ApproveTransaction
{
    public class ApproveTransactionCommand: IRequest<TransactionDTO>
    {
        public Guid TransactionId { get; private set; }
        public ApproveTransactionCommand(Guid transactionId)
        {
            TransactionId = transactionId;
        }
    }
}
