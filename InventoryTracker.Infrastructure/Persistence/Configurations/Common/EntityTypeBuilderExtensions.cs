using InventoryTracker.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Infrastructure.Persistence.Configurations.Common
{
    public static class EntityTypeBuilderExtensions
    {
        public static void ConfigureAuditable<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : AuditableEntity
        {
            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .IsRequired(false)
                .HasMaxLength(150);

            builder.Property(x => x.UpdatedAt)
                .IsRequired(false);

            builder.Property(x => x.UpdatedBy)
                .IsRequired(false)
                .HasMaxLength(150);
        }

        public static void ConfigureSoftDeletable<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : SoftDeletableEntity
        {
            builder.ConfigureAuditable();

            builder.Property(x => x.IsDeleted)
                .IsRequired();

            builder.Property(x => x.DeletedAt)
                .IsRequired(false);

            builder.Property(x => x.DeletedBy)
                .IsRequired(false)
                .HasMaxLength(150);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
