using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Domain.Common;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser>, IAppDbContext
{
    private readonly ICurrentUserService _currentUserService;
    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<TransactionItem> TransactionItems => Set<TransactionItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentUser = _currentUserService.Email ?? _currentUserService.UserId ?? "system";
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = currentUser;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = currentUser;
            }
            else if (entry.State == EntityState.Deleted && entry.Entity is SoftDeletableEntity softDeletable)
            {
                entry.State = EntityState.Modified;
                softDeletable.IsActive = false;
                softDeletable.DeletedAt = now;
                softDeletable.DeletedBy = currentUser;
                softDeletable.UpdatedAt = now;
                softDeletable.UpdatedBy = currentUser;
            }

            if (entry.Entity is SoftDeletableEntity softEntity && entry.State == EntityState.Modified)
            {
                var isActiveProperty = entry.Property(nameof(SoftDeletableEntity.IsActive));
                var wasActive = (bool)isActiveProperty.OriginalValue!;
                var isActive = (bool)isActiveProperty.CurrentValue!;

                if (wasActive && !isActive && softEntity.DeletedAt is null)
                {
                    softEntity.DeletedAt = now;
                    softEntity.DeletedBy = currentUser;
                }
                if(!wasActive && isActive)
                {
                    softEntity.DeletedAt = null;
                    softEntity.DeletedBy = null;
                }
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}
