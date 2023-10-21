using DealNotifier.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ItemType = DealNotifier.Core.Domain.Entities.ItemType;

namespace DealNotifier.Persistence.Configuration
{
    public class ItemTypeConfiguration : AuditableEntityConfiguration<ItemType>
    {
        public override void Configure(EntityTypeBuilder<ItemType> builder)
        {
            #region Table

            builder.ToTable("ItemType");

            #endregion Table

            #region Properties

            builder.Property(x => x.Name)
                    .HasColumnType("nvarchar(20)")
                    .IsRequired();

            #endregion Properties

            #region Data Seeding

            builder.HasData(ItemTypeSeed.Data);

            #endregion Data Seeding

            base.Configure(builder);
        }
    }
}