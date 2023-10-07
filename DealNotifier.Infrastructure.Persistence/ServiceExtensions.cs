using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using DealNotifier.Infrastructure.Persistence.Repositories;
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
                services.AddDbContext<ApplicationDbContext>(DbContextSetup.InMemoryOptions);
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(DbContextSetup.Configure(configuration));
            }

            #endregion DbContext

            #region Dependency Injection

            services.AddScoped(typeof(IGenericRepositoryAsync<,>), typeof(GenericRepositoryAsync<,>));
            services.AddScoped<IItemRepositoryAsync, ItemRepositoryAsync>();
            services.AddScoped<IBanKeywordRepositoryAsync, BanKeywordRepositoryAsync>();
            services.AddScoped<IBanLinkRepositoryAsync, BanLinkRepositoryAsync>();

            #endregion Dependency Injection
        }
    }
}