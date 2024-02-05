using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Interfaces.Services.Items;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.Services.Items;
using DealNotifier.Core.Application.Setups;
using DealNotifier.Core.Application.Setups.Swagger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DealNotifier.Core.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiVersioning(ApiVersionSetup.Configure);
            services.AddAuthorization();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();
            services.AddCors(CorsSetup.Configure);
            services.AddEndpointsApiExplorer();
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddSwaggerGen(SwaggerGenSetup.Configure);
            services.AddVersionedApiExplorer(VersionedApiExplorerSetup.Configure);



            #region Dependency Injection

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddScoped(typeof(IAsyncService<>), typeof(ServiceBase<>));
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IBanKeywordService, BanKeywordService>();
            services.AddScoped<IBanLinkService, BanLinkService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IConditionService, ConditionService>();
            services.AddScoped<IItemTypeService, ItemTypeService>();
            services.AddScoped<INotificationCriteriaService, NotificationCriteriaService>();
            services.AddScoped<IPhoneCarrierService, PhoneCarrierService>();
            services.AddScoped<IPhoneUnlockToolService, PhoneUnlockToolService>();
            services.AddScoped<IOnlineStoreService, OnlineStoreService>();
            services.AddScoped<IStockStatusService, StockStatusService>();
            services.AddScoped<IUnlockabledPhoneService, UnlockabledPhoneService>();
            services.AddScoped<IUnlockabledPhonePhoneUnlockToolService, UnlockabledPhonePhoneUnlockToolService>();
            services.AddScoped<IUnlockabledPhonePhoneCarrierService, UnlockabledPhonePhoneCarrierService>();
            services.AddScoped<IUnlockProbabilityService, UnlockProbabilityService>();
            services.AddScoped<IItemManagerService, ItemManagerService>();
            services.AddScoped<IItemValidationService, ItemValidationService>();
            services.AddScoped<ICacheDataService, CacheDataService>();
            services.AddScoped<IItemNotificationService, ItemNotificationService>();
            services.AddScoped<IUnlockVerificationService, UnlockVerificationService>();

            #endregion Dependency Injection
        }
    }
}