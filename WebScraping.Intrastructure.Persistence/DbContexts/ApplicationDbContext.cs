using Microsoft.EntityFrameworkCore;
using WebScraping.Core.Domain.Entities;
using WebScraping.Core.Application.Models;
using Type = WebScraping.Core.Domain.Entities.Type;

namespace WebScraping.Intrastructure.Persistence.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() 
        {

        }

        #region DbSets
        public DbSet<Item> Items { get; set; }
        public DbSet<BlackList> BlackLists { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Supported> Supporteds { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SpBlackListResponse> SpBlackList { get; set; }


        #endregion DbSets

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Item
            modelBuilder.Entity<Item>(entity =>
            {
                //TABLE
                entity.ToTable("Item");

                //PROPERTIES
                entity.Property(x => x.Name)
                    .HasColumnType("VARCHAR(max)")
                    .IsRequired();

                entity.Property(x => x.Price)
                    .HasColumnType("DECIMAL(13,2)")
                    .IsRequired();

                entity.Property(x => x.Image)
                    .HasColumnType("varchar(MAX)")
                    .IsRequired();

                entity.Property(x => x.Link)
                    .HasColumnType("nvarchar(450)")
                    .IsRequired();

                entity.Property(x => x.OldPrice)
                    .HasColumnType("DECIMAL(13,2)")
                    .HasDefaultValueSql("0")
                    .IsRequired();

                entity.Property(x => x.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()")
                    .IsRequired();

                entity.Property(x => x.LastModified)
                    .HasColumnType("datetime");

                entity.Property(x => x.Saving)
                    .HasColumnType("DECIMAL(13,2)")
                    .HasDefaultValueSql("0")
                    .IsRequired();

                entity.Property(x => x.SavingsPercentage)
                    .HasColumnType("DECIMAL(13,2)")
                    .HasDefaultValueSql("0")
                    .IsRequired();

                //KEYS
                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.Status)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.StatusId)
                .HasConstraintName("FK_Item_Status");

                entity.HasOne(x => x.Condition)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.ConditionId)
                .HasConstraintName("FK_Item_Condition");

                entity.HasOne(x => x.Type)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.TypeId)
                .HasConstraintName("FK_Item_Type");

                entity.HasOne(x => x.Shop)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.ShopId)
                .HasConstraintName("FK_Item_Shop");

            });
            #endregion Item

            #region Contidion
            modelBuilder.Entity<Condition>(entity =>
            {
                //TABLE
                entity.ToTable("Condition");

                //PROPERTIES
                entity.Property(x => x.Name)
                    .HasColumnType("VARCHAR(15)")
                    .IsRequired();

                //KEYS
                entity.HasKey(x => x.Id);

            });
            #endregion Contidion

            #region Shop
            modelBuilder.Entity<Shop>(entity =>
            {
                //TABLE
                entity.ToTable("Shop");

                //PROPERTIES
                entity.Property(x => x.Name)
                    .HasColumnType("VARCHAR(15)")
                    .IsRequired();

                //KEYS
                entity.HasKey(x => x.Id);

            });
            #endregion Shop

            #region Type
            modelBuilder.Entity<Type>(entity =>
            {
                //TABLE
                entity.ToTable("Type");

                //PROPERTIES
                entity.Property(x => x.Name)
                    .HasColumnType("VARCHAR(20)")
                    .IsRequired();
                //KEYS
                entity.HasKey(x => x.Id);

            });
            #endregion Type

            #region Status
            modelBuilder.Entity<Status>(entity =>
            {
                //TABLE
                entity.ToTable("Status");

                //PROPERTIES
                entity.Property(x => x.Name)
                    .HasColumnType("VARCHAR(20)")
                    .IsRequired();

                //KEYS
                entity.HasKey(x => x.Id);

            });
            #endregion Status

            #region User
            modelBuilder.Entity<User>(entity =>
            {
                //TABLE
                entity.ToTable("User");

                //PROPERTIES
                entity.Property(x => x.Name)
                    .HasColumnType("VARCHAR(50)")
                    .IsRequired();

                entity.Property(x => x.UserName)
                   .HasColumnType("VARCHAR(50)")
                   .IsRequired();

                entity.Property(x => x.Password)
                   .HasColumnType("VARCHAR(50)")
                   .IsRequired();

                //KEYS
                entity.HasKey(x => x.Id);

            });
            #endregion User 
            
            #region BlackList
            modelBuilder.Entity<BlackList>(entity =>
            {
                //TABLE
                entity.ToTable("BlackList");

                //PROPERTIES
                entity.Property(x => x.Date)
                    .HasColumnType("DATETIME")
                    .IsRequired();

                entity.Property(x => x.Link)
                    .HasColumnType("VARCHAR(max)")
                    .IsRequired();

                //KEYS
                entity.HasKey(x => x.Id);

            });
            #endregion BlackList

            #region Supported
            modelBuilder.Entity<Supported>(entity =>
            {
                //TABLE
                entity.ToTable("Supported");

                //PROPERTIES
                entity.Property(x => x.ModelName)
                    .HasColumnType("VARCHAR(30)")
                    .IsRequired();

                entity.Property(x => x.ModelNumber)
                    .HasColumnType("VARCHAR(30)")
                    .IsRequired();

                entity.Property(x => x.Tool)
                    .HasColumnType("VARCHAR(30)")
                    .IsRequired();

                entity.Property(x => x.SupportedVersion)
                    .HasColumnType("VARCHAR(MAX)")
                    .IsRequired();

                entity.Property(x => x.SupportedBit)
                    .HasColumnType("VARCHAR(MAX)");

                entity.Property(x => x.Comment)
                    .HasColumnType("VARCHAR(30)");

                entity.Property(x => x.Carrier)
                    .HasColumnType("VARCHAR(10)");

                //KEYS
                entity.HasKey(x => x.Id);

            });
            #endregion Supported


            //STORED PROCEDURE
            #region SpBlackList
            modelBuilder.Entity<SpBlackListResponse>(entity =>
            {
                entity.HasNoKey();
            });
            #endregion SpBlackList
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=LocalHost;Database=WebScraping;User=pupyfrias;Password=08143611;Max Pool Size=2000;MultipleActiveResultSets=True");
            base.OnConfiguring(optionsBuilder);
        }

    }
}
