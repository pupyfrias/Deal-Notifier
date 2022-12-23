using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScraping.Infrastructure.Identity.Models;
using WebScraping.Infrastructure.Identity.Seeds;

namespace WebScraping.Infrastructure.Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("User");
            builder.HasData(UserSeed.data);
        }
    }
}