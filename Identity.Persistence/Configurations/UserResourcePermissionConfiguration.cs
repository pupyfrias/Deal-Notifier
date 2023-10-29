using Identity.Persistence.Entity;
using Identity.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Persistence.Configurations
{
    public class UserResourcePermissionConfiguration : IEntityTypeConfiguration<UserResourcePermission>
    {
        public void Configure(EntityTypeBuilder<UserResourcePermission> builder)
        {

            builder.HasKey(entity => new { entity.ResourceId, entity.PermissionId, entity.UserId });

            builder.HasOne(entity => entity.Resource)
                   .WithMany(r => r.UserResourcePermissions)
                   .HasForeignKey(entity => entity.ResourceId);

            builder
                .HasOne(entity => entity.Permission)
                .WithMany(p => p.UserResourcePermissions)
                .HasForeignKey(entity => entity.PermissionId);


            builder.HasOne(entity => entity.User)
                .WithMany(u => u.UserResourcePermissions)
                .HasForeignKey(entity => entity.UserId);


            builder.HasData(UserResourcePermissionSeed.Data);
        }
    }
}