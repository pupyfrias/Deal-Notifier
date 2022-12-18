using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.Extensions;
using WebScraping.Infrastructure.Persistence.Seeds;

namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            #region Table
            builder.ToTable("Status");
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
            builder.HasData(StatusSeed.data);
            #endregion Data Seeding
        }
    }
}
