using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Infrastructure.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(x => x.AddressId);

            builder.Property(x => x.Street)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.HouseNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.ApartmentNumber)
                .IsRequired(false)
                .HasMaxLength(20);

            builder.Property(x => x.PostalCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasOne(x => x.Country)
                .WithMany(c => c.Addresses)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(x => x.CountryId);

            builder.ConfigureAuditable();
        }
    }
}
