using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockStatus = DealNotifier.Core.Application.Enums.StockStatus;

namespace DealNotifier.Persistence.Configuration
{
    public class ItemConfiguration : AuditableEntityConfiguration<Item>
    {
        public override void Configure(EntityTypeBuilder<Item> builder)
        {
            #region Table

            builder.ToTable("Item");

            #endregion Table

            #region Properties

            builder.Property(x => x.PublicId)
                   .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.Name)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnType("decimal(13,2)")
                .IsRequired();

            builder.Property(x => x.Image)
                .HasColumnType("nvarchar(MAX)")
                .IsRequired();

            builder.Property(x => x.Link)
                .HasColumnType("nvarchar(254)")
                .IsRequired();

            builder.Property(x => x.OldPrice)
                .HasColumnType("decimal(13,2)")
                .HasDefaultValueSql("0")
                .IsRequired();

            builder.Property(x => x.Saving)
                .HasColumnType("decimal(13,2)")
                .HasDefaultValueSql("0")
                .IsRequired();

            builder.Property(x => x.SavingsPercentage)
                .HasColumnType("decimal(13,2)")
                .HasDefaultValueSql("0")
                .IsRequired();

            builder.Property(x => x.Notify)
                .HasColumnType("BIT")
                .HasDefaultValueSql("1")
                .IsRequired();

            builder.Property(x => x.IsAuction)
                .HasColumnType("BIT")
                .HasDefaultValueSql("0")
                .IsRequired();

            builder.Property(x => x.Notified)
                .IsRequired(false);


            builder.Property(x => x.ItemEndDate)
            .HasColumnType("DateTime")
            .IsRequired(false);


            builder.Property(x => x.BidCount)
           .HasColumnType("Int");

            builder.Property(x => x.UnlockabledPhoneId)
                .IsRequired(false);

            #endregion Properties

            #region Keys

            builder.HasOne(x => x.Brand)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.BrandId)
            .HasConstraintName("FK_Item_Brand");


            builder.HasOne(x => x.StockStatus)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.StockStatusId)
            .HasConstraintName("FK_Item_StockStatus");

            builder.HasOne(x => x.Condition)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.ConditionId)
            .HasConstraintName("FK_Item_Condition");

            builder.HasOne(x => x.ItemType)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.ItemTypeId)
            .HasConstraintName("FK_Item_ItemType");

            builder.HasOne(x => x.OnlineStore)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.OnlineStoreId)
            .HasConstraintName("FK_Item_OnlineStore");

            builder.HasOne(x => x.UnlockabledPhone)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.UnlockabledPhoneId)
            .HasConstraintName("FK_Item_UnlockabledPhone")
            .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.UnlockProbability)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.UnlockProbabilityId)
            .HasConstraintName("FK_Item_UnlockProbability");

            #endregion Keys

            #region Indexes
            builder.HasIndex(e => e.Link)
            .IsUnique();

            builder.HasIndex(e => e.PublicId)
            .IsUnique();
            #endregion Indexes
            
            base.Configure(builder);
        }
    }
}