using DealNotifier.Core.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealNotifier.Persistence.Configuration
{
    public abstract class AuditableEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : AuditableEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreatedBy)
                  .HasDefaultValue("default");

            builder.Property(e => e.Created)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.LastModifiedBy);

            builder.Property(e => e.LastModified);
        }
    }
}