using DealNotifier.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealNotifier.Persistence.Configuration
{
    public class BanLinkConfiguration : AuditableEntityConfiguration<BanLink>
    {
        public override void Configure(EntityTypeBuilder<BanLink> builder)
        {
            #region Table

            builder.ToTable("BanLink");

            #endregion Table

            #region Properties

            builder.Property(x => x.Link)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            #endregion Properties

        }
    }
}