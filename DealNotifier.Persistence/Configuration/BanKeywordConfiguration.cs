﻿using DealNotifier.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealNotifier.Persistence.Configuration
{
    public class BanKeywordConfiguration : AuditableEntityConfiguration<BanKeyword>
    {
        public override void Configure(EntityTypeBuilder<BanKeyword> builder)
        {
            #region Table

            builder.ToTable("BanKeyword");

            #endregion Table

            #region Properties

            builder.Property(x => x.Keyword)
                .HasColumnType("nvarchar(50)")
                .IsRequired();

            builder.HasIndex(x => x.Keyword)
                .IsUnique();
            #endregion Properties

        }
    }
}