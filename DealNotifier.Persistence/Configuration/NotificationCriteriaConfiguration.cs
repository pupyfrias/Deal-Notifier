using DealNotifier.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealNotifier.Persistence.Configuration
{
    public class NotificationCriteriaConfiguration : AuditableEntityConfiguration<NotificationCriteria>
    {
        public override void Configure(EntityTypeBuilder<NotificationCriteria> builder)
        {
            #region Table

            builder.ToTable("NotificationCriteria");

            #endregion Table

            #region Properties

            builder.Property(x => x.IncludeKeywords)
                   .HasColumnType("nvarchar(MAX)")
                   .IsRequired();

            builder.Property(x => x.ExcludeKeywords)
                   .HasColumnType("nvarchar(MAX)")
                   .IsRequired();

            builder.Property(x => x.ConditionId)
                  .HasColumnType("INT")
                  .IsRequired();

            builder.Property(x => x.MaxPrice)
                  .HasColumnType("decimal(13,2)")
                  .IsRequired();

            #endregion Properties

            #region Keys
            builder.HasOne(x => x.Condition)
            .WithMany(x => x.ConditionToNotifies)
            .HasForeignKey(x => x.ConditionId)
            .HasConstraintName("FK_ConditionsToModify_Condition");

            #endregion Keys

            base.Configure(builder);
        }
    }
}