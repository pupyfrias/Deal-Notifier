using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.Extensions;
using WebScraping.Infrastructure.Persistence.Seeds;

namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public class ConditionConfiguration : IEntityTypeConfiguration<Condition>
    {
        public void Configure(EntityTypeBuilder<Condition> builder)
        {
           

            #region Table
            builder.ToTable("Condition");
            #endregion Table

            #region Properties
            builder.Property(x => x.Name)
                   .HasColumnType("VARCHAR(15)")
                   .IsRequired();
            #endregion Properties

            #region AuditableBaseEntity Properties
            builder.AddAutableBaseEntityProperties();
            #endregion AuditableBaseEntity Properties

            #region Keys
            builder.HasKey(x => x.Id);
            #endregion Keys

            #region Data Seeding
            builder.HasData(ConditionSeed.data);
            #endregion Data Seeding
        }




        //Expression<Func<TEntity, TProperty>> propertyExpression<TEntity,TProperty> = prop=>{



        



        
    }
}
