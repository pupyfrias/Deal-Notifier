using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebScraping.Core.Application.DTOs;
using WebScraping.Core.Application.Extensions;
using WebScraping.Core.Application.Models;
using WebScraping.Core.Domain.Common;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.Configuration;
using Action = WebScraping.Core.Application.Emuns.Action;
using Type = WebScraping.Core.Domain.Entities.Type;


namespace WebScraping.Infrastructure.Persistence.DbContexts
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly string _userName = "default";
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContext):base(options)
        {
            _userName = httpContext.HttpContext.GetUserName();
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
        public DbSet<BlackListDTO> SpBlackList { get; set; }
        public DbSet<Audit> AuditLogs { get; set; }

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
            SetEntry();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            SetEntry();
            return base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            base.OnConfiguring(optionsBuilder);
        }

        private void SetEntry()
        {
            var auditEntryList = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry();
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserName = _userName;
                auditEntryList.Add(auditEntry);

                #region AuditableBaseEntity
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _userName;
                        entry.Entity.Created = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _userName;
                        entry.Entity.LastModified = DateTime.Now;
                        break;
                    default:
                        break;
                }
                #endregion AuditableBaseEntity

                #region AuditLogs
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;

                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.Action = Action.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.Action = Action.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified && 
                                property.OriginalValue?.ToString() != property.CurrentValue?.ToString())
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.Action = Action.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }

                            break;
                    }
                }
                #endregion AuditLogs
            }

            foreach (var auditEntry in auditEntryList)
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }

        }
    }
}
