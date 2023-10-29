using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Persistence.DbContexts;
using Catalog.Persistence.Repositories;
using Catalog.Persistence.Setup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Persistence
{
    public static class ServiceExtensions
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(DbContextSetup.InMemoryOptions);
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(DbContextSetup.Configure(configuration));
            }

            #endregion DbContext

            #region Dependency Injection
            services.AddScoped<IBanKeywordRepository, BanKeywordRepository>();
            services.AddScoped<IBanLinkRepository, BanLinkRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IConditionRepository, ConditionRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IItemTypeRepository, ItemTypeRepository>();
            services.AddScoped<INotificationCriteriaRepository, NotificationCriteriaRepository>();
            services.AddScoped<IOnlineStoreRepository, OnlineStoreRepository>();
            services.AddScoped<IPhoneCarrierRepository, PhoneCarrierRepository>();
            services.AddScoped<IPhoneUnlockToolRepository, PhoneUnlockToolRepository>();
            services.AddScoped<IStockStatusRepository, StockStatusRepository>();
            services.AddScoped<IUnlockabledPhoneRepository, UnlockabledPhoneRepository>();
            services.AddScoped<IUnlockabledPhonePhoneUnlockToolRepository, UnlockabledPhonePhoneUnlockToolRepository>();
            services.AddScoped<IUnlockabledPhonePhoneCarrierRepository, UnlockabledPhonePhoneCarrierRepository>();
            services.AddScoped<IUnlockProbabilityRepository, UnlockProbabilityRepository>();

            #endregion Dependency Injection
        }
    }
}