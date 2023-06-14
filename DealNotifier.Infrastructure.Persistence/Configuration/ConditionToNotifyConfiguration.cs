using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Infrastructure.Persistence.Configuration
{
    public class ConditionsToNotifyConfiguration : AuditableEntityConfiguration<ConditionsToNotify, int>
    {
        public override void Configure(EntityTypeBuilder<ConditionsToNotify> builder)
        {
            #region Table

            builder.ToTable("ConditionsToNotify");

            #endregion Table

            #region Properties

            builder.Property(x => x.Keywords)
                   .HasColumnType("VARCHAR(MAX)")
                   .IsRequired();

            builder.Property(x => x.ConditionId)
                  .HasColumnType("INT")
                  .IsRequired();

            builder.Property(x => x.MaxPrice)
                  .HasColumnType("DECIMAL(13,2)")
                  .IsRequired();

            #endregion Properties

            #region Keys

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Condition)
            .WithMany(x => x.ConditionToNotifies)
            .HasForeignKey(x => x.ConditionId)
            .HasConstraintName("FK_ConditionsToModify_Condition");

            #endregion Keys

            base.Configure(builder);
        }
    }
}