    using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealNotifier.Infrastructure.Persistence.Configuration
{
    public class PhoneUnlockToolConfiguration : AuditableEntityConfiguration<PhoneUnlockTool, int>
    {
        public override void Configure(EntityTypeBuilder<PhoneUnlockTool> builder)
        {
            #region Table

            builder.ToTable("PhoneUnlockTool");

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

            builder.HasData(UnlockToolSeed.Data);

            #endregion Data Seeding

            base.Configure(builder);
        }
    }
}