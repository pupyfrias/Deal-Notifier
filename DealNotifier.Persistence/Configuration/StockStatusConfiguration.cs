using DealNotifier.Core.Domain.Entities;
using DealNotifier.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealNotifier.Persistence.Configuration
{
    public class StockStatusConfiguration : AuditableEntityConfiguration<StockStatus>
    {
        public override void Configure(EntityTypeBuilder<StockStatus> builder)
        {
            #region Table

            builder.ToTable("StockStatus");

            #endregion Table

            #region Properties

            builder.Property(x => x.Name)
                   .HasColumnType("nvarchar(20)")
                   .IsRequired();

            #endregion Properties

            #region Data Seeding

            builder.HasData(StatusSeed.Data);

            #endregion Data Seeding

            base.Configure(builder);
        }
    }
}