using Serilog;
using System.Reflection;
using DealNotifier.Core.Application.Contracts.Repositories;
using DealNotifier.Core.Application.Contracts.Services;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.SetupOptions;
using DealNotifier.Core.Domain.Configs;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using DealNotifier.Infrastructure.Persistence.Repositories;
using WorkerService.Samkey;



var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

if (environment != "Development")
{
    Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
}


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddHttpContextAccessor();
        services.AddMemoryCache();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddSingleton(new ApplicationDbContext());

        #region Repositories
        services.AddSingleton(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
        services.AddSingleton<IUnlockableRepositoryAsync, UnlockableRepositoryAsync>();
        services.AddSingleton<IPhoneCarrierRepositoryAsync, PhoneCarrierRepositoryAsync>();
        services.AddSingleton<IUnlockableUnlockToolRepositoryAsync, UnlockableUnlockToolRepositoryAsync>();
        services.AddSingleton<IUnlockablePhoneCarrierRepositoryAsync, UnlockablePhoneCarrierRepositoryAsync>();

        #endregion Repositories

        #region Services
        services.AddSingleton(typeof(IGenericServiceAsync<>), typeof(GenericServiceAsync<>));
        services.AddSingleton<IUnlockableServiceAsync, UnlockableServiceAsync>();
        services.AddSingleton<IPhoneCarrierServiceAsync, PhoneCarrierServiceAsync>();
        services.AddSingleton<IUnlockableUnlockToolServiceAsync, UnlockableUnlockToolServiceAsync>();
        services.AddSingleton<IUnlockablePhoneCarrierServiceAsync, UnlockablePhoneCarrierServiceAsync>();

        #endregion Services

        #region Configure
        services.Configure<SamkeyUrlConfig>(hostContext.Configuration.GetSection("SamkeyUrlConfig"));

        #endregion Configure


    })
    .UseSerilog(SeriLog.Options)
    .Build();

await host.RunAsync();