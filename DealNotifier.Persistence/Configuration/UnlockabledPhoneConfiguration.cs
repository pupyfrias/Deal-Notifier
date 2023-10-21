using DealNotifier.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealNotifier.Persistence.Configuration
{
    public class UnlockabledPhoneConfiguration : AuditableEntityConfiguration<UnlockabledPhone>
    {
        public override void Configure(EntityTypeBuilder<UnlockabledPhone> builder)
        {
            #region Table

            builder.ToTable("UnlockabledPhone");

            #endregion Table

            #region Properties

            builder.Property(x => x.ModelName)
                   .HasColumnType("nvarchar(50)")
                   .IsRequired();

            builder.Property(x => x.ModelNumber)
                .HasColumnType("nvarchar(15)")
                .IsRequired();

            builder.Property(x => x.Comment)
                .HasColumnType("nvarchar(50)")
                .IsRequired(false);

            #endregion Properties

            #region Index

            builder.HasIndex(x => x.ModelNumber)
            .IsUnique();

            #endregion Index

            base.Configure(builder);
        }
    }
}