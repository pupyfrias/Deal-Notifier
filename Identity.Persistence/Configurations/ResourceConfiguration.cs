using Identity.Persistence.Entity;
using Identity.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Persistence.Configurations
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.HasIndex(entity=> entity.Name);
            builder.HasData(ResourceSeed.Data);
        }
    }
}