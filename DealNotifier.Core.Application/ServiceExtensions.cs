using DealNotifier.Core.Application.Contracts.Services;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.SetupOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DealNotifier.Core.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(Cors.Options);
            services.AddAuthorization();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(SwaggerGen.Options);
            services.AddHttpContextAccessor();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();
            services.AddMemoryCache();

            #region Dependency Injection

            services.AddScoped(typeof(IGenericServiceAsync<>), typeof(GenericServiceAsync<>));
            services.AddScoped<IItemServiceAsync, ItemServiceAsync>();
            
            #endregion Dependency Injection
        }
    }
}