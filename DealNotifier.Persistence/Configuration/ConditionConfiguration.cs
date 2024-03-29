﻿using Catalog.Domain.Entities;
using Catalog.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Persistence.Configuration
{
    public class ConditionConfiguration : AuditableEntityConfiguration<Condition>
    {
        public override void Configure(EntityTypeBuilder<Condition> builder)
        {
            #region Table

            builder.ToTable("Condition");

            #endregion Table

            #region Properties

            builder.Property(x => x.Name)
                   .HasColumnType("nvarchar(15)")
                   .IsRequired();

            #endregion Properties

            #region Data Seeding

            builder.HasData(ConditionSeed.Data);

            #endregion Data Seeding

            base.Configure(builder);
        }
    }
}