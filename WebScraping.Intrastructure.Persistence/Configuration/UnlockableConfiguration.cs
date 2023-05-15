using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public class UnlockableConfiguration : AuditableEntityConfiguration<Unlockable, int>
    {
        public override void Configure(EntityTypeBuilder<Unlockable> builder)
        {
            #region Table

            builder.ToTable("Unlockable");

            #endregion Table

            #region Properties

            builder.Property(x => x.ModelName)
                   .HasColumnType("VARCHAR(50)")
                   .IsRequired();

            builder.Property(x => x.ModelNumber)
                .HasColumnType("VARCHAR(15)")
                .IsRequired();

            builder.Property(x => x.Comment)
                .HasColumnType("VARCHAR(50)")
                .IsRequired(false);


            #endregion Properties

            #region Keys

            builder.HasKey(x => x.Id);

            #endregion Keys

            #region Index
            builder.HasIndex(x => x.ModelNumber)
            .IsUnique();
            #endregion Index

            base.Configure(builder);
        }
    }
}