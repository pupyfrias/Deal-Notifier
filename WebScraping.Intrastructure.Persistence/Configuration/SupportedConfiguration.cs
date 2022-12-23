using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public class SupportedConfiguration : AuditableBaseEntityConfiguration<Supported>
    {
        public override void Configure(EntityTypeBuilder<Supported> builder)
        {
            #region Table

            builder.ToTable("Supported");

            #endregion Table

            #region Properties

            builder.Property(x => x.ModelName)
                   .HasColumnType("VARCHAR(30)")
                   .IsRequired();

            builder.Property(x => x.ModelNumber)
                .HasColumnType("VARCHAR(30)")
                .IsRequired();

            builder.Property(x => x.Tool)
                .HasColumnType("VARCHAR(30)")
                .IsRequired();

            builder.Property(x => x.SupportedVersion)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();

            builder.Property(x => x.SupportedBit)
                .HasColumnType("VARCHAR(MAX)");

            builder.Property(x => x.Comment)
                .HasColumnType("VARCHAR(30)");

            builder.Property(x => x.Carrier)
                .HasColumnType("VARCHAR(10)");

            #endregion Properties

            #region Keys

            builder.HasKey(x => x.Id);

            #endregion Keys

            base.Configure(builder);
        }
    }
}