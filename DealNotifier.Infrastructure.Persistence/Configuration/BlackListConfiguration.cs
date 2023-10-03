﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Infrastructure.Persistence.Configuration
{
    public class BlackListConfiguration : AuditableEntityConfiguration<BlackList, int>
    {
        public override void Configure(EntityTypeBuilder<BlackList> builder)
        {
            #region Table

            builder.ToTable("BlackList");

            #endregion Table

            #region Properties

            builder.Property(x => x.Link)
                .HasColumnType("VARCHAR(max)")
                .IsRequired();

            #endregion Properties

            #region Keys

            builder.HasKey(x => x.Id);

            #endregion Keys
        }
    }
}