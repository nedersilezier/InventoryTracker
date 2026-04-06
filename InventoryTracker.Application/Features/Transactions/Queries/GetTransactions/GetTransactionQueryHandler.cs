using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Transactions.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Transactions.Queries.GetTransactions
{
    public class GetTransactionQueryHandler: IRequestHandler<GetTransactionQuery, List<TransactionDTO>>
    {
        private readonly IAppDbContext _context;
        public GetTransactionQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<TransactionDTO>> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Select(t => new TransactionDTO
                {
                    TransactionId = t.TransactionId,
                    TransactionDate = t.TransactionDate,
                    Type = t.Type,
                    Status = t.Status,
                    ClientId = t.ClientId,
                    ClientName = t.Client == null ? null : t.Client.Name,
                    SourceWarehouseId = t.SourceWarehouseId,
                    DestinationWarehouseId = t.DestinationWarehouseId,
                    ReferenceNumber = t.ReferenceNumber,
                    Notes = t.Notes,
                })
                .ToListAsync(cancellationToken);
        }
    }
}
