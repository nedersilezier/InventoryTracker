using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Infrastructure.Persistence.Configurations
{
    public class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("Stocks");

            builder.HasKey(x => x.StockId);

            builder.Property(x => x.Quantity)
                .HasColumnType("decimal(18,2)");

            builder.HasIndex(x => new { x.WarehouseId, x.ItemId })
                .IsUnique();

            builder.HasOne(x => x.Warehouse)
                .WithMany(x => x.Stocks)
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Item)
                .WithMany(x => x.Stocks)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ConfigureAuditable();
        }
    }
}
