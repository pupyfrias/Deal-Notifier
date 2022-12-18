using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.Extensions;

namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public class BlackListConfiguration : IEntityTypeConfiguration<BlackList>
    {
        public void Configure(EntityTypeBuilder<BlackList> builder)
        {
            #region Table
            builder.ToTable("BlackList");
            #endregion Table

            #region Properties
            builder.Property(x => x.Date)
                   .HasColumnType("DATETIME")
                   .IsRequired();

            builder.Property(x => x.Link)
                .HasColumnType("VARCHAR(max)")
                .IsRequired();
            #endregion Properties

            #region AuditableBaseEntity Properties
            builder.AddAutableBaseEntityProperties();
            #endregion AuditableBaseEntity Properties

            #region Keys
            builder.HasKey(x => x.Id);
            #endregion Keys
        }
    }
}
