using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealNotifier.Infrastructure.Persistence.Configuration
{
    public class PhoneCarrierConfiguration : AuditableEntityConfiguration<PhoneCarrier, int>
    {
        public override void Configure(EntityTypeBuilder<PhoneCarrier> builder)
        {
            #region Table

            builder.ToTable("PhoneCarrier");

            #endregion Table

            #region Properties

            builder.Property(x => x.Name)
                    .HasColumnType("VARCHAR(30)")
                    .IsRequired();

            builder.Property(x => x.ShortName)
                    .HasColumnType("VARCHAR(5)")
                    .IsRequired();

            #endregion Properties

            #region Keys

            builder.HasKey(x => x.Id);

            #endregion Keys

            #region Index

            builder.HasIndex(x => x.ShortName)
            .IsUnique();

            #endregion Index

            #region Data Seeding

            builder.HasData(PhoneCarrierSeed.Data);

            #endregion Data Seeding

            base.Configure(builder);
        }
    }
}