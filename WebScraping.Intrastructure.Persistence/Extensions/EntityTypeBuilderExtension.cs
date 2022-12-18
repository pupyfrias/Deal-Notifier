using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebScraping.Infrastructure.Persistence.Extensions
{
    public static class EntityTypeBuilderExtension
    {
        public static void AddAutableBaseEntityProperties(this EntityTypeBuilder builder)
        {
            builder.Property("CreatedBy")
                   .HasDefaultValue("default");

            builder.Property("Created")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property("LastModifiedBy");

            builder.Property("LastModified");
        }
    }
}
