using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebScraping.Core.Application.Mappings;
using WebScraping.Core.Application.SetupOptions;

namespace WebScraping.Core.Application
{
    public static class ServiceExtesions
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(Cors.Options);
            services.AddControllers().AddOData(OData.Options);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(SwaggerGen.Options);
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(AutomapperConfig));
        }
    }
}
