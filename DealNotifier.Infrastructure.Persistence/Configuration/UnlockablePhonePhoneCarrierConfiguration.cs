using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Infrastructure.Persistence.Configuration
{
    public class UnlockablePhonePhoneCarrierConfiguration : IEntityTypeConfiguration<UnlockablePhonePhoneCarrier>
    {
        public void Configure(EntityTypeBuilder<UnlockablePhonePhoneCarrier> builder)
        {
            #region Table

            builder.ToTable("UnlockablePhonePhoneCarrier");

            #endregion Table


            #region Keys

            builder.HasKey(x => new { x.UnlockablePhoneId, x.PhoneCarrierId });

            builder.HasOne(x => x.PhoneCarrier)
                .WithMany(x => x.UnlockablePhoneCarriers)
                .HasForeignKey(x => x.PhoneCarrierId);

            builder.HasOne(x => x.Unlockable)
                .WithMany(x => x.UnlockablePhoneCarriers)
                .HasForeignKey(x => x.UnlockablePhoneId);

            #endregion Keys


        }
    }
}