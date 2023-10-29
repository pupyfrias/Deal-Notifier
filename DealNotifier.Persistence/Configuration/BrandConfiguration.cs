using Catalog.Domain.Entities;
using Catalog.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Persistence.Configuration
{
    public class BrandConfiguration : AuditableEntityConfiguration<Brand>
    {
        public override void Configure(EntityTypeBuilder<Brand> builder)
        {
            #region Table

            builder.ToTable("Brand");

            #endregion Table

            #region Properties

            builder.Property(x => x.Name)
                    .HasColumnType("nvarchar(30)")
                    .IsRequired();

            #endregion Properties

            #region Data Seeding

            builder.HasData(BrandSeed.Data);

            #endregion Data Seeding

            base.Configure(builder);
        }
    }
}