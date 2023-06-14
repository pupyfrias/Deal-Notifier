using DealNotifier.Infrastructure.Persistence.DbContexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DealNotifier.Infrastructure.Persistence
{
    public static class ServiceExtensions
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(DbContextAction.InMemoryOptions);
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(DbContextAction.DbOptions(configuration));
            }

            #endregion DbContext
        }
    }
}