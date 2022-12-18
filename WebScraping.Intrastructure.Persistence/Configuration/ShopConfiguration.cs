using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.Extensions;
using WebScraping.Infrastructure.Persistence.Seeds;

namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public class ShopConfiguration : IEntityTypeConfiguration<Shop>
    {
        public void Configure(EntityTypeBuilder<Shop> builder)
        {

            #region Table
            builder.ToTable("Shop");
            #endregion Table

            #region Properties
            builder.Property(x => x.Name)
                    .HasColumnType("VARCHAR(15)")
                    .IsRequired();
            #endregion Properties

            #region AuditableBaseEntity Properties
            builder.AddAutableBaseEntityProperties();
            #endregion AuditableBaseEntity Properties

            #region Keys
            builder.HasKey(x => x.Id);
            #endregion Keys

            #region Data Seeding
            builder.HasData(ShopSeed.data);
            #endregion Data Seeding
        }
    }
}