using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.Seeds;

namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public class UnlockToolConfiguration : AuditableEntityConfiguration<UnlockTool, int>
    {
        public override void Configure(EntityTypeBuilder<UnlockTool> builder)
        {
            #region TTool

            builder.ToTable("UnlockTool");

            #endregion

            #region Properties

            builder.Property(x => x.Name)
                   .HasColumnType("VARCHAR(20)")
                   .IsRequired();

            #endregion Properties

            #region Keys

            builder.HasKey(x => x.Id);

            #endregion Keys

            #region Data Seeding

            builder.HasData(UnlockToolSeed.data);

            #endregion Data Seeding

            base.Configure(builder);
        }
    }
}