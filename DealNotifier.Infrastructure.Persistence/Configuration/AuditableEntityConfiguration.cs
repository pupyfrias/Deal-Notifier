﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Infrastructure.Persistence.Configuration
{
    public abstract class AuditableEntityConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity> where TEntity : AuditableEntity<TKey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(x => x.CreatedBy)
                  .HasDefaultValue("default");

            builder.Property(x => x.Created)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.LastModifiedBy);

            builder.Property(x => x.LastModified);
        }
    }
}