using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Infrastructure.Persistence.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Items");

            builder.HasKey(x => x.ItemId);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.SKU)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.UnitOfMeasure)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.CreditValue)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(x => x.Weight)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.HasIndex(x => x.SKU)
                .IsUnique();

            builder.ConfigureSoftDeletable();
        }
    }
}
