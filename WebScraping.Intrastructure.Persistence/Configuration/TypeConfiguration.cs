using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScraping.Infrastructure.Persistence.Extensions;
using WebScraping.Infrastructure.Persistence.Seeds;
using Type = WebScraping.Core.Domain.Entities.Type;
namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public class TypeConfiguration : IEntityTypeConfiguration<Type>
    {
        public void Configure(EntityTypeBuilder<Type> builder)
        {
            #region Table
            builder.ToTable("Type");
            #endregion Table

            #region Properties
            builder.Property(x => x.Name)
                    .HasColumnType("VARCHAR(20)")
                    .IsRequired();
            #endregion Properties

            #region AuditableBaseEntity Properties
            builder.AddAutableBaseEntityProperties();
            #endregion AuditableBaseEntity Properties

            #region Keys
            builder.HasKey(x => x.Id);
            #endregion Keys

            #region Data Seeding
            builder.HasData(TypeSeed.data);
            #endregion Data Seeding
        }
    }
}
