using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Infrastructure.Repositories
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly AppDbContext _context;
        public TransactionsRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            return Task.CompletedTask;
        }
        public async Task<Transaction?> GetTransactionWithItemsByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Transactions.Include(t => t.TransactionItems).FirstOrDefaultAsync(t => t.TransactionId == id, cancellationToken);
        }
        public async Task<Transaction?> GetTransactionByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionId == id, cancellationToken);
        }
        public Task AddTransactionItem(TransactionItem transactionItem)
        {
            _context.TransactionItems.Add(transactionItem);
            return Task.CompletedTask;
        }
    }
}
