using DealNotifier.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealNotifier.Infrastructure.Persistence.Configuration
{
    public class BanLinkConfiguration : AuditableEntityConfiguration<BanLink, int>
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

            #region Keys

            builder.HasKey(x => x.Id);

            #endregion Keys
        }
    }
}