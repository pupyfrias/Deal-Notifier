using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public class BlackListConfiguration : AuditableBaseEntityConfiguration<BlackList>
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

            #region Keys
            builder.HasKey(x => x.Id);
            #endregion Keys
        }
    }
}
