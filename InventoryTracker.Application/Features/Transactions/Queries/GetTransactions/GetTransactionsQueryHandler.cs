using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Application.Features.Transactions.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Transactions.Queries.GetTransactions
{
    public class GetTransactionsQueryHandler: IRequestHandler<GetTransactionsQuery, List<TransactionListDTO>>
    {
        private readonly IAppDbContext _context;
        public GetTransactionsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<TransactionListDTO>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Select(t => new TransactionListDTO
                {
                    TransactionId = t.TransactionId,
                    TransactionDate = t.TransactionDate,
                    Type = t.Type,
                    Status = t.Status,
                    ClientId = t.ClientId,
                    ClientName = t.Client == null ? null : t.Client.Name,
                    SourceWarehouseId = t.SourceWarehouseId,
                    DestinationWarehouseId = t.DestinationWarehouseId,
                    SourceWarehouseNameSnapshot = t.SourceWarehouseNameSnapshot,
                    DestinationWarehouseNameSnapshot = t.DestinationWarehouseNameSnapshot,
                    ReferenceNumber = t.ReferenceNumber,
                    Notes = t.Notes,
                })
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync(cancellationToken);
        }
    }
}
