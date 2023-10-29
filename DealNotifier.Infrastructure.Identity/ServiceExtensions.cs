using Catalog.Application.Interfaces.Services;
using Catalog.Domain.Configs;
using DealNotifier.Infrastructure.Identity.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DealNotifier.Infrastructure.Identity
{
    public static class ServiceExtensions
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {


            #region Configure

            services.Configure<JWTConfig>(configuration.GetSection("JWTConfig"));
            services.Configure<SecurityServiceConfig>(configuration.GetSection("SecurityServiceConfig"));

            #endregion Configure



            #region Dependency injection

            services.AddScoped<IAuthService, AuthService>();

            #endregion Dependency injection

           
        }
    }
}