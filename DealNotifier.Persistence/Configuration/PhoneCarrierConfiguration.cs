﻿using Catalog.Domain.Entities;
using Catalog.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Persistence.Configuration
{
    public class PhoneCarrierConfiguration : AuditableEntityConfiguration<PhoneCarrier>
    {
        public override void Configure(EntityTypeBuilder<PhoneCarrier> builder)
        {
            #region Table

            builder.ToTable("PhoneCarrier");

            #endregion Table

            #region Properties

            builder.Property(x => x.Name)
                    .HasColumnType("nvarchar(30)")
                    .IsRequired();

            builder.Property(x => x.ShortName)
                    .HasColumnType("nvarchar(5)")
                    .IsRequired();

            #endregion Properties

            #region Index

            builder.HasIndex(x => x.ShortName)
            .IsUnique();

            #endregion Index

            #region Data Seeding

            builder.HasData(PhoneCarrierSeed.Data);

            #endregion Data Seeding

            base.Configure(builder);
        }
    }
}