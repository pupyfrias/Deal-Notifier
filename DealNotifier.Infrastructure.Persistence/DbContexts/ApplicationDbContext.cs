﻿using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Domain.Contracts;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Action = DealNotifier.Core.Application.Enums.Action;
using ItemType = DealNotifier.Core.Domain.Entities.ItemType;

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
        public DbSet<BanKeyword> BanKeywords { get; set; }
        public DbSet<BanLink> BanLinks { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<NotificationCriteria> NotificationCriteria { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<PhoneCarrier> PhoneCarriers { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<StockStatus> StockStatuses { get; set; }
        public DbSet<ItemType> Types { get; set; }
        public DbSet<UnlockabledPhonePhoneCarrier> UnlockabledPhonePhoneCarriers { get; set; }
        public DbSet<UnlockabledPhone> UnlockabledPhones { get; set; }
        public DbSet<UnlockabledPhoneUnlockTool> UnlockablePhoneUnlockTools { get; set; }
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DealNotifierConnection"));
            //optionsBuilder.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BanKeywordConfiguration());
            modelBuilder.ApplyConfiguration(new BanLinkConfiguration());
            modelBuilder.ApplyConfiguration(new BrandConfiguration());
            modelBuilder.ApplyConfiguration(new ConditionConfiguration());
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationCriteriaConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneCarrierConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneUnlockToolConfiguration());
            modelBuilder.ApplyConfiguration(new ShopConfiguration());
            modelBuilder.ApplyConfiguration(new StockStatusConfiguration());
            modelBuilder.ApplyConfiguration(new ItemTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UnlockabledPhoneConfiguration());
            modelBuilder.ApplyConfiguration(new UnlockabledPhoneUnlockToolConfiguration());
            modelBuilder.ApplyConfiguration(new UnlockabledPhonePhoneCarrierConfiguration());
            modelBuilder.ApplyConfiguration(new UnlockProbabilityConfiguration());
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