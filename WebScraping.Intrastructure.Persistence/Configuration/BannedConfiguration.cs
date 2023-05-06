﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public class BannedConfiguration : AuditableBaseEntityConfiguration<Banned>
    {
        public override void Configure(EntityTypeBuilder<Banned> builder)
        {
            #region Table

            builder.ToTable("Banned");

            #endregion Table

            #region Properties

            builder.Property(x => x.Keyword)
                .HasColumnType("VARCHAR(50)")
                .IsRequired();

            #endregion Properties

            #region Keys

            builder.HasKey(x => x.Id);

            #endregion Keys
        }
    }
}