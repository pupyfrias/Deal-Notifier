using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Persistence.Configuration
{
    public class UnlockabledPhonePhoneUnlockToolConfiguration : IEntityTypeConfiguration<UnlockabledPhonePhoneUnlockTool>
    {
        public void Configure(EntityTypeBuilder<UnlockabledPhonePhoneUnlockTool> builder)
        {
            #region Table

            builder.ToTable("UnlockabledPhonePhoneUnlockTool");

            #endregion Table

            #region Keys

            builder.HasKey(x => new { x.PhoneUnlockToolId, x.UnlockabledPhoneId });

            builder.HasOne(x => x.PhoneUnlockTool)
                .WithMany(x => x.UnlockableUnlockTools)
                .HasForeignKey(x => x.PhoneUnlockToolId);

            builder.HasOne(x => x.UnlockabledPhone)
                .WithMany(x => x.UnlockabledPhoneUnlockTool)
                .HasForeignKey(x => x.UnlockabledPhoneId);

            #endregion Keys
        }
    }
}