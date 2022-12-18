using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebScraping.Infrastructure.Persistence.DbContexts;

namespace WebScraping.Infrastructure.Persistence
{
    public static class ServiceExtensions
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(DbContext.InMemoryOptions);
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(DbContext.Options(configuration));
            }
            #endregion DbContext

        }
    }
}
