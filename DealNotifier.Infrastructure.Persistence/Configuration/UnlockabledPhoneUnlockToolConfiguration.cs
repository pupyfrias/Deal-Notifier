using DealNotifier.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealNotifier.Infrastructure.Persistence.Configuration
{
    public class UnlockabledPhoneUnlockToolConfiguration : IEntityTypeConfiguration<UnlockabledPhoneUnlockTool>
    {
        public void Configure(EntityTypeBuilder<UnlockabledPhoneUnlockTool> builder)
        {
            #region Table

            builder.ToTable("UnlockabledPhoneUnlockTool");

            #endregion Table

            #region Keys

            builder.HasKey(x => new { x.PhoneUnlockToolId, x.UnlockablePhoneId });

            builder.HasOne(x => x.PhoneUnlockTool)
                .WithMany(x => x.UnlockableUnlockTools)
                .HasForeignKey(x => x.PhoneUnlockToolId);

            builder.HasOne(x => x.UnlockablePhone)
                .WithMany(x => x.UnlockableUnlockTools)
                .HasForeignKey(x => x.PhoneUnlockToolId);

            #endregion Keys
        }
    }
}