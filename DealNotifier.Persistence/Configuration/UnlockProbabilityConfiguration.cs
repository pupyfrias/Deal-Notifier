﻿using Catalog.Domain.Entities;
using Catalog.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Persistence.Configuration
{
    public class UnlockProbabilityConfiguration : AuditableEntityConfiguration<UnlockProbability>
    {
        public override void Configure(EntityTypeBuilder<UnlockProbability> builder)
        {
            #region Table

            builder.ToTable("UnlockProbability");

            #endregion Table

            #region Properties

            builder.Property(x => x.Name)
                   .HasColumnType("nvarchar(15)")
                   .IsRequired();

            #endregion Properties

            #region Data Seeding

            builder.HasData(UnlockProbabilitySeed.Data);

            #endregion Data Seeding

            base.Configure(builder);
        }
    }
}