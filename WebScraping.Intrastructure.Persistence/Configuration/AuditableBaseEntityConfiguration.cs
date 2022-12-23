using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScraping.Core.Domain.Common;

namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public abstract class AuditableBaseEntityConfiguration<Entity> : IEntityTypeConfiguration<Entity> where Entity : AuditableBaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<Entity> builder)
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