using DealNotifier.Infrastructure.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Infrastructure.Persistence.Configuration
{
    public class ShopConfiguration : AuditableEntityConfiguration<Shop, int>
    {
        public override void Configure(EntityTypeBuilder<Shop> builder)
        {
            #region Table

            builder.ToTable("Shop");

            #endregion Table

            #region Properties

            builder.Property(x => x.Name)
                    .HasColumnType("VARCHAR(15)")
                    .IsRequired();

            #endregion Properties

            #region Keys

            builder.HasKey(x => x.Id);

            #endregion Keys

            #region Data Seeding

            builder.HasData(ShopSeed.data);

            #endregion Data Seeding

            base.Configure(builder);
        }
    }
}