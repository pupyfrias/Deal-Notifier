using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public class UnlockablePhoneCarrierConfiguration : IEntityTypeConfiguration<UnlockablePhoneCarrier>
    {
        public void Configure(EntityTypeBuilder<UnlockablePhoneCarrier> builder)
        {
            #region Table

            builder.ToTable("UnlockablePhoneCarrier");

            #endregion Table


            #region Keys

            builder.HasKey(x=> new {x.UnlockableId, x.PhoneCarrierId});

            builder.HasOne(x => x.PhoneCarrier)
                .WithMany(x => x.UnlockablePhoneCarriers)
                .HasForeignKey(x => x.PhoneCarrierId);

            builder.HasOne(x => x.Unlockable)
                .WithMany(x => x.UnlockablePhoneCarriers)
                .HasForeignKey(x => x.UnlockableId);

            #endregion Keys


        }
    }
}