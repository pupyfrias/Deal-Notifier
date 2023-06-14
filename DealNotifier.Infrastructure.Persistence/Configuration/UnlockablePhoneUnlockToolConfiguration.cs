using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Infrastructure.Persistence.Configuration
{
    public class UnlockablePhoneUnlockToolConfiguration : IEntityTypeConfiguration<UnlockablePhoneUnlockTool>
    {
        public void Configure(EntityTypeBuilder<UnlockablePhoneUnlockTool> builder)
        {
            #region Table

            builder.ToTable("UnlockablePhoneUnlockTool");

            #endregion Table


            #region Keys

            builder.HasKey(x => new { x.UnlockablePhoneId, x.UnlockToolId });

            builder.HasOne(x => x.UnlockTool)
                .WithMany(x => x.UnlockableUnlockTools)
                .HasForeignKey(x => x.UnlockToolId);

            builder.HasOne(x => x.Unlockable)
                .WithMany(x => x.UnlockableUnlockTools)
                .HasForeignKey(x => x.UnlockablePhoneId);

            #endregion Keys


        }
    }
}