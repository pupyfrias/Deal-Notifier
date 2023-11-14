using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels;
using DealNotifier.Core.Domain.Common;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Action = DealNotifier.Core.Application.Enums.Action;
using ItemType = DealNotifier.Core.Domain.Entities.ItemType;

namespace DealNotifier.Persistence.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string _userName = "default";

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }



        #region DbSets

        public DbSet<Audit> AuditLogs { get; set; }
        public DbSet<BanKeyword> BanKeywords { get; set; }
        public DbSet<BanLink> BanLinks { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<NotificationCriteria> NotificationCriteria { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<PhoneCarrier> PhoneCarriers { get; set; }
        public DbSet<OnlineStore> Shops { get; set; }
        public DbSet<StockStatus> StockStatuses { get; set; }
        public DbSet<ItemType> Types { get; set; }
        public DbSet<UnlockabledPhonePhoneCarrier> UnlockabledPhonePhoneCarriers { get; set; }
        public DbSet<UnlockabledPhone> UnlockabledPhones { get; set; }
        public DbSet<UnlockabledPhonePhoneUnlockTool> UnlockabledPhonePhoneUnlockTools { get; set; }
        public DbSet<UnlockProbability> UnlockProbability { get; set; }

        #endregion DbSets

        public override int SaveChanges()
        {
            SetEntry();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetEntry();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        private void SetEntry()
        {
            var auditEntryList = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
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