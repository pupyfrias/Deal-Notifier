using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public class UnlockableUnlockToolConfiguration : IEntityTypeConfiguration<UnlockableUnlockTool>
    {
        public void Configure(EntityTypeBuilder<UnlockableUnlockTool> builder)
        {
            #region Table

            builder.ToTable("UnlockableUnlockTool");

            #endregion Table


            #region Keys

            builder.HasKey(x=> new {x.UnlockableId, x.UnlockToolId});

            builder.HasOne(x => x.UnlockTool)
                .WithMany(x => x.UnlockableUnlockTools)
                .HasForeignKey(x => x.UnlockToolId);

            builder.HasOne(x => x.Unlockable)
                .WithMany(x => x.UnlockableUnlockTools)
                .HasForeignKey(x => x.UnlockableId);

            #endregion Keys


        }
    }
}