using DealNotifier.Infrastructure.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ItemType = DealNotifier.Core.Domain.Entities.ItemType;

namespace DealNotifier.Infrastructure.Persistence.Configuration
{
    public class ItemTypeConfiguration : AuditableEntityConfiguration<ItemType, int>
    {
        public override void Configure(EntityTypeBuilder<ItemType> builder)
        {
            #region Table

            builder.ToTable("ItemType");

            #endregion Table

            #region Properties

            builder.Property(x => x.Name)
                    .HasColumnType("VARCHAR(20)")
                    .IsRequired();

            #endregion Properties

            #region Keys

            builder.HasKey(x => x.Id);

            #endregion Keys

            #region Data Seeding

            builder.HasData(ItemTypeSeed.Data);

            #endregion Data Seeding

            base.Configure(builder);
        }
    }
}