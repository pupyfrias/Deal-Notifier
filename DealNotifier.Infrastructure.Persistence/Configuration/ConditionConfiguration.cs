using DealNotifier.Infrastructure.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Infrastructure.Persistence.Configuration
{
    public class ConditionConfiguration : AuditableEntityConfiguration<Condition, int>
    {
        public override void Configure(EntityTypeBuilder<Condition> builder)
        {
            #region Table

            builder.ToTable("Condition");

            #endregion Table

            #region Properties

            builder.Property(x => x.Name)
                   .HasColumnType("VARCHAR(15)")
                   .IsRequired();

            builder.Property(x => x.Id)
                .ValueGeneratedNever();


            #endregion Properties

            #region Keys
            builder.HasKey(x => x.Id);
            #endregion Keys

            #region Data Seeding

            builder.HasData(ConditionSeed.data);

            #endregion Data Seeding

            base.Configure(builder);
        }
    }
}