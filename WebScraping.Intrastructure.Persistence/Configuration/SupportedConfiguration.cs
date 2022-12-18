using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.Extensions;

namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public class SupportedConfiguration : IEntityTypeConfiguration<Supported>
    {
        public void Configure(EntityTypeBuilder<Supported> builder)
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

            #region AuditableBaseEntity Properties
            builder.AddAutableBaseEntityProperties();
            #endregion AuditableBaseEntity Properties

            #region Keys
            builder.HasKey(x => x.Id);
            #endregion Keys
        }
    }
}
