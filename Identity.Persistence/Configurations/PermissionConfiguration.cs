using Identity.Persistence.Entity;
using Identity.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Persistence.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.HasIndex(entity=> entity.Name);
            builder.HasData(PermissionSeed.Data);
        }
    }
}