using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealNotifier.Infrastructure.Persistence.Configuration
{
    public class OnlineStoreConfiguration : AuditableEntityConfiguration<OnlineStore, int>
    {
        public override void Configure(EntityTypeBuilder<OnlineStore> builder)
        {
            #region Table

            builder.ToTable("OnlineStore");

            #endregion Table

            #region Properties

            builder.Property(x => x.Name)
                    .HasColumnType("nvarchar(15)")
                    .IsRequired();

            #endregion Properties

            #region Keys

            builder.HasKey(x => x.Id);

            #endregion Keys

            #region Data Seeding

            builder.HasData(OnlineStoreSeed.Data);

            #endregion Data Seeding

            base.Configure(builder);
        }
    }
}