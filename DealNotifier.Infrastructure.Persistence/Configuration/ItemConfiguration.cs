using DealNotifier.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealNotifier.Infrastructure.Persistence.Configuration
{
    public class ItemConfiguration : AuditableEntityConfiguration<Item, Guid>
    {
        public override void Configure(EntityTypeBuilder<Item> builder)
        {
            #region Table

            builder.ToTable("Item");

            #endregion Table

            #region Properties

            builder.Property(x => x.Id)
                   .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.Name)
                    .HasColumnType("VARCHAR(max)")
                    .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnType("DECIMAL(13,2)")
                .IsRequired();

            builder.Property(x => x.Image)
                .HasColumnType("varchar(MAX)")
                .IsRequired();

            builder.Property(x => x.Link)
                .HasColumnType("nvarchar(450)")
                .IsRequired();

            builder.Property(x => x.OldPrice)
                .HasColumnType("DECIMAL(13,2)")
                .HasDefaultValueSql("0")
                .IsRequired();

            builder.Property(x => x.Saving)
                .HasColumnType("DECIMAL(13,2)")
                .HasDefaultValueSql("0")
                .IsRequired();

            builder.Property(x => x.SavingsPercentage)
                .HasColumnType("DECIMAL(13,2)")
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

            builder.Property(x => x.ModelNumber)
            .HasColumnType("varchar(25)");

            builder.Property(x => x.ModelName)
           .HasColumnType("varchar(25)");

            builder.Property(x => x.ItemEndDate)
            .HasColumnType("DateTime")
            .IsRequired(false);

            builder.Property(x => x.BrandId)
           .HasDefaultValueSql("1")
           .IsRequired();

            builder.Property(x => x.PhoneCarrierId)
           .HasDefaultValueSql("1");

            builder.Property(x => x.BidCount)
           .HasColumnType("Int");

            #endregion Properties

            #region Keys

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Status)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.StatusId)
            .HasConstraintName("FK_Item_Status");

            builder.HasOne(x => x.Condition)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.ConditionId)
            .HasConstraintName("FK_Item_Condition");

            builder.HasOne(x => x.Type)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.TypeId)
            .HasConstraintName("FK_Item_Type");

            builder.HasOne(x => x.OnlineStore)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.ShopId)
            .HasConstraintName("FK_Item_Shop");

            builder.HasOne(x => x.Brand)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.BrandId)
            .HasConstraintName("FK_Item_Brand");

            builder.HasOne(x => x.PhoneCarrier)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.PhoneCarrierId)
            .HasConstraintName("FK_Item_PhoneCarrier");

            builder.HasOne(x => x.UnlockProbability)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.UnlockProbabilityId)
            .HasConstraintName("FK_Item_UnlockProbability");

            builder.HasIndex(e => e.Link)
            .IsUnique();

            #endregion Keys

            base.Configure(builder);
        }
    }
}