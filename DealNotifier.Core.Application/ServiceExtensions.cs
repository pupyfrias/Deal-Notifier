using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Services;
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
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
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
            services.AddScoped(typeof(IGenericServiceAsync<,>), typeof(GenericServiceAsync<,>));
            services.AddScoped<IItemServiceAsync, ItemServiceAsync>();
            services.AddScoped<IBanKeywordServiceAsync, BanKeywordServiceAsync>();
            services.AddScoped<IBanLinkServiceAsync, BanLinkServiceAsync>();
            services.AddScoped<IBrandServiceAsync, BrandServiceAsync>();
            services.AddScoped<IConditionServiceAsync, ConditionServiceAsync>();


            #endregion Dependency Injection
        }
    }
}