﻿using Catalog.Application.Interfaces.Services;
using Catalog.Application.Interfaces.Services.Items;
using Catalog.Application.Services;
using Catalog.Application.Services.Items;
using Catalog.Domain.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Catalog.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddHttpContextAccessor();
            services.AddMemoryCache();




            #region Dependency Injection

            //services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
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

            services.AddSingleton<SecurityServiceConfig>();
        }
    }
}