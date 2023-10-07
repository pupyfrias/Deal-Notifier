using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.Setups;
using DealNotifier.Core.Domain.Configs;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using DealNotifier.Infrastructure.Persistence.Repositories;
using Serilog;
using System.Reflection;
using WorkerService.T_Unlock_WebScraping;

string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.SetBasePath(baseDirectory)
                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
        })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddHttpContextAccessor();
        services.AddMemoryCache();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddSingleton(new ApplicationDbContext());

        #region Repositories

        services.AddSingleton(typeof(IGenericRepositoryAsync<,>), typeof(GenericRepositoryAsync<,>));
        services.AddSingleton<IUnlockableRepositoryAsync, UnlockableRepositoryAsync>();
        services.AddSingleton<IPhoneCarrierRepositoryAsync, PhoneCarrierRepositoryAsync>();
        services.AddSingleton<IUnlockableUnlockToolRepositoryAsync, UnlockableUnlockToolRepositoryAsync>();
        services.AddSingleton<IUnlockablePhoneCarrierRepositoryAsync, UnlockablePhoneCarrierRepositoryAsync>();

        #endregion Repositories

        #region Services

        services.AddSingleton(typeof(IGenericServiceAsync<,>), typeof(GenericServiceAsync<,>));
        services.AddSingleton<IUnlockableServiceAsync, UnlockableServiceAsync>();
        services.AddSingleton<IPhoneCarrierServiceAsync, PhoneCarrierServiceAsync>();
        services.AddSingleton<IUnlockableUnlockToolServiceAsync, UnlockableUnlockToolServiceAsync>();
        services.AddSingleton<IUnlockablePhoneCarrierServiceAsync, UnlockablePhoneCarrierServiceAsync>();

        #endregion Services

        #region Configure

        services.Configure<TUnlockUrlConfig>(hostContext.Configuration.GetSection("TUnlockUrlConfig"));

        #endregion Configure
    })
    .UseSerilog(SeriLogSetup.Configure)
    .Build();
var env = host.Services.GetRequiredService<IHostEnvironment>();
if (env.IsProduction())
{
    Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
}
await host.RunAsync();