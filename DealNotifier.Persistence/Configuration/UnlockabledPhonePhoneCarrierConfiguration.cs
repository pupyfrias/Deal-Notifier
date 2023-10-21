using DealNotifier.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealNotifier.Persistence.Configuration
{
    public class UnlockabledPhonePhoneCarrierConfiguration : IEntityTypeConfiguration<UnlockabledPhonePhoneCarrier>
    {
        public void Configure(EntityTypeBuilder<UnlockabledPhonePhoneCarrier> builder)
        {
            #region Table

            builder.ToTable("UnlockabledPhonePhoneCarrier");

            #endregion Table

            #region Keys

            builder.HasKey(x => new { x.UnlockabledPhoneId, x.PhoneCarrierId });

            builder.HasOne(x => x.PhoneCarrier)
                .WithMany(x => x.UnlockablePhoneCarriers)
                .HasForeignKey(x => x.PhoneCarrierId);

            builder.HasOne(x => x.UnlockabledPhone)
                .WithMany(x => x.UnlockabledPhonePhoneCarrier)
                .HasForeignKey(x => x.UnlockabledPhoneId);

            #endregion Keys
        }
    }
}