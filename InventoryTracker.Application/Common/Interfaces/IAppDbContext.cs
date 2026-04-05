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
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
