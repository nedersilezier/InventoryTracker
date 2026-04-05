using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(x => x.TransactionId);

            builder.Property(x => x.Type)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.TransactionDate)
                .IsRequired();

            builder.Property(x => x.ReferenceNumber)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(x => x.Notes)
                .IsRequired(false)
                .HasMaxLength(1000);

            builder.Property(x => x.CancelledAt)
                .IsRequired(false);

            builder.Property(x => x.CancelledBy)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(x => x.CancellationReason)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.HasOne(x => x.Client)
                .WithMany()
                .HasForeignKey(x => x.ClientId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SourceWarehouse)
                .WithMany()
                .HasForeignKey(x => x.SourceWarehouseId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DestinationWarehouse)
                .WithMany()
                .HasForeignKey(x => x.DestinationWarehouseId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ConfigureAuditable();

            builder.HasIndex(x => x.ClientId);
            builder.HasIndex(x => x.SourceWarehouseId);
            builder.HasIndex(x => x.DestinationWarehouseId);
            builder.HasIndex(x => x.TransactionDate);
        }
    }
}
