using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DealNotifier.Core.Application.DTOs;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.Models;
using DealNotifier.Core.Domain.Common;
using DealNotifier.Core.Domain.Contracts;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.Configuration;
using Action = DealNotifier.Core.Application.Enums.Action;
using Type = DealNotifier.Core.Domain.Entities.Type;

namespace DealNotifier.Infrastructure.Persistence.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string _userName = "default";

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContext) : base(options)
        {
            _userName = httpContext.HttpContext.GetUserName();
        }

        public ApplicationDbContext()
        {
        }

        #region DbSets

        public DbSet<Audit> AuditLogs { get; set; }
        public DbSet<Banned> Banned { get; set; }
        public DbSet<BlackList> BlackLists { get; set; }
        public DbSet<BlackListDto> SpBlackList { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<UnlockablePhone> UnlockablePhones { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<ConditionsToNotify> ConditionsToNotify { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<UnlockablePhonePhoneCarrier> UnlockablePhoneCarriers { get; set; }
        public DbSet<UnlockablePhoneUnlockTool> UnlockableUnlockTools { get; set; }
        public DbSet<PhoneCarrier> PhoneCarriers { get; set; }
        public DbSet<UnlockProbability> UnlockProbability { get; set; }


        #endregion DbSets

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BannedConfiguration());
            modelBuilder.ApplyConfiguration(new BlackListConfiguration());
            modelBuilder.ApplyConfiguration(new BrandConfiguration());
            modelBuilder.ApplyConfiguration(new ConditionConfiguration());
            modelBuilder.ApplyConfiguration(new ConditionsToNotifyConfiguration());
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneCarrierConfiguration());
            modelBuilder.ApplyConfiguration(new ShopConfiguration());
            modelBuilder.ApplyConfiguration(new SpBlackListResponseConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());
            modelBuilder.ApplyConfiguration(new TypeConfiguration());
            modelBuilder.ApplyConfiguration(new UnlockablePhoneConfiguration());
            modelBuilder.ApplyConfiguration(new UnlockablePhonePhoneCarrierConfiguration());
            modelBuilder.ApplyConfiguration(new UnlockablePhoneUnlockToolConfiguration());
            modelBuilder.ApplyConfiguration(new UnlockToolConfiguration());
            modelBuilder.ApplyConfiguration(new UnlockProbabilityConfiguration());
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

            optionsBuilder.UseSqlServer(config.GetConnectionString("DealNotifierConnection"));
            //optionsBuilder.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
            base.OnConfiguring(optionsBuilder);
        }

        private void SetEntry()
        {
            var auditEntryList = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry();
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserName = _userName;
                auditEntryList.Add(auditEntry);

                #region AuditableEntity<int>

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

                #endregion AuditableEntity<int>

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