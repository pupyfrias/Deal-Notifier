using DealNotifier.Core.Application.SetupOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DealNotifier.Core.Application
{
    public static class ServiceExtesions
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
        }
    }
}