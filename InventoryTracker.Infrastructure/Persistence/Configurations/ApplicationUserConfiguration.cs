using InventoryTracker.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName)
                .HasMaxLength(256);
            builder.Property(u => u.LastName)
                .HasMaxLength(256);
            builder.HasIndex(u => u.NormalizedUserName).IsUnique();
            builder.HasIndex(u => u.NormalizedEmail).IsUnique();
        }
    }
}
