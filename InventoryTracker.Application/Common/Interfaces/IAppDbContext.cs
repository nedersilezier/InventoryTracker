using System;
using System.Collections.Generic;
using System.Text;
using InventoryTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Item> Items { get; }
        DbSet<Client> Clients { get; }
        DbSet<Warehouse> Warehouses { get; }
        DbSet<Stock> Stocks { get; }
        DbSet<Transaction> Transactions { get; }
        DbSet<TransactionItem> TransactionItems { get; }
        DbSet<Country> Countries { get; }
        DbSet<Address> Addresses { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
