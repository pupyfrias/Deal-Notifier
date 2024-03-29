﻿using Catalog.Domain.Entities;
using Catalog.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Persistence.Configuration
{
    public class OnlineStoreConfiguration : AuditableEntityConfiguration<OnlineStore>
    {
        public override void Configure(EntityTypeBuilder<OnlineStore> builder)
        {
            #region Table

            builder.ToTable("OnlineStore");

            #endregion Table

            #region Properties

            builder.Property(x => x.Name)
                    .HasColumnType("nvarchar(15)")
                    .IsRequired();

            #endregion Properties

            #region Data Seeding

            builder.HasData(OnlineStoreSeed.Data);

            #endregion Data Seeding

            base.Configure(builder);
        }
    }
}