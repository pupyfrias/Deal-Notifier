﻿using DealNotifier.Infrastructure.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Infrastructure.Persistence.Configuration
{
    public class StatusConfiguration : AuditableEntityConfiguration<Status, int>
    {
        public override void Configure(EntityTypeBuilder<Status> builder)
        {
            #region Table

            builder.ToTable("Status");

            #endregion Table

            #region Properties

            builder.Property(x => x.Name)
                   .HasColumnType("VARCHAR(20)")
                   .IsRequired();

            builder.Property(x => x.Id)
               .ValueGeneratedNever();

            #endregion Properties

            #region Keys
            builder.HasKey(x => x.Id);
            #endregion Keys

            #region Data Seeding

            builder.HasData(StatusSeed.data);

            #endregion Data Seeding

            base.Configure(builder);
        }
    }
}