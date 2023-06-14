using DealNotifier.Infrastructure.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Type = DealNotifier.Core.Domain.Entities.Type;

namespace DealNotifier.Infrastructure.Persistence.Configuration
{
    public class TypeConfiguration : AuditableEntityConfiguration<Type, int>
    {
        public override void Configure(EntityTypeBuilder<Type> builder)
        {
            #region Table

            builder.ToTable("Type");

            #endregion Table

            #region Properties

            builder.Property(x => x.Name)
                    .HasColumnType("VARCHAR(20)")
                    .IsRequired();

            #endregion Properties

            #region Keys

            builder.HasKey(x => x.Id);

            #endregion Keys

            #region Data Seeding

            builder.HasData(TypeSeed.data);

            #endregion Data Seeding

            base.Configure(builder);
        }
    }
}