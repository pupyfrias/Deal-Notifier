using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public class ItemConfiguration : AuditableBaseEntityConfiguration<Item>
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

            builder.Property(x => x.Created)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()")
                .IsRequired();

            builder.Property(x => x.LastModified)
                .HasColumnType("datetime");

            builder.Property(x => x.Saving)
                .HasColumnType("DECIMAL(13,2)")
                .HasDefaultValueSql("0")
                .IsRequired();

            builder.Property(x => x.SavingsPercentage)
                .HasColumnType("DECIMAL(13,2)")
                .HasDefaultValueSql("0")
                .IsRequired();

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

            builder.HasOne(x => x.Shop)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.ShopId)
            .HasConstraintName("FK_Item_Shop");

            #endregion Keys

            base.Configure(builder);
        }
    }
}