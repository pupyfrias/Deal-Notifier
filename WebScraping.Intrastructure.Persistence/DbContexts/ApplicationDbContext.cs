using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using WebScraping.Core.Application.Extensions;
using WebScraping.Core.Application.Models;
using WebScraping.Core.Domain.Common;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.Configuration;
using Type = WebScraping.Core.Domain.Entities.Type;


namespace WebScraping.Infrastructure.Persistence.DbContexts
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly string _user;
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContext):base(options)
        {
            _user = httpContext.HttpContext.GetUserName();
        }

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
        public DbSet<SpBlackListResponse> SpBlackList { get; set; }
        #endregion DbSets

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new ConditionConfiguration());
            modelBuilder.ApplyConfiguration(new ShopConfiguration());
            modelBuilder.ApplyConfiguration(new TypeConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());
            modelBuilder.ApplyConfiguration(new BlackListConfiguration());
            modelBuilder.ApplyConfiguration(new SupportedConfiguration());
            modelBuilder.ApplyConfiguration(new SpBlackListResponseConfiguration());
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entity in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        entity.Entity.CreatedBy = _user;
                        break;
                    case EntityState.Modified:
                        entity.Entity.LastModifiedBy = _user;
                        entity.Entity.LastModified = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            base.OnConfiguring(optionsBuilder);
        }
    }
}
