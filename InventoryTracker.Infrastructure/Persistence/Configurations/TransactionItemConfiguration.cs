using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Infrastructure.Persistence.Configurations
{
    public class TransactionItemConfiguration : IEntityTypeConfiguration<TransactionItem>
    {
        public void Configure(EntityTypeBuilder<TransactionItem> builder)
        {
            builder.ToTable("TransactionItems");

            builder.HasKey(x => x.TransactionItemId);

            builder.Property(x => x.Quantity)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.UnitCreditValue)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(x => x.TotalCreditValue)
                .HasComputedColumnSql("[Quantity] * [UnitCreditValue]", stored: true);

            builder.HasOne(x => x.Transaction)
                .WithMany(x => x.TransactionItems)
                .HasForeignKey(x => x.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Item)
                .WithMany(x => x.TransactionItems)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ConfigureAuditable();
        }
    }
}
