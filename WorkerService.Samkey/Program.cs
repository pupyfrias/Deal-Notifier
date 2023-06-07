using Serilog;
using System.Reflection;
using WebScraping.Core.Application.Contracts.Repositories;
using WebScraping.Core.Application.Contracts.Services;
using WebScraping.Core.Application.Services;
using WebScraping.Core.Application.SetupOptions;
using WebScraping.Core.Domain.Configs;
using WebScraping.Infrastructure.Persistence.DbContexts;
using WebScraping.Infrastructure.Persistence.Repositories;
using WorkerService.Samkey;


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

var env = host.Services.GetRequiredService<IHostEnvironment>();
if (env.IsProduction())
{
    Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
}

await host.RunAsync();