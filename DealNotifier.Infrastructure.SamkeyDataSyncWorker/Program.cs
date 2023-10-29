using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.Services;
using Catalog.Domain.Configs;
using Catalog.Persistence.DbContexts;
using Catalog.Persistence.Repositories;
using Catalog.Persistence.Setup;
using DealNotifier.Infrastructure.SamkeyDataSyncWorker;
using DealNotifier.Infrastructure.SamkeyDataSyncWorker.Interfaces;
using DealNotifier.Infrastructure.SamkeyDataSyncWorker.Services;
using Identity.Application.SetupOptions;
using Serilog;
using System.Reflection;

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
        services.AddHttpClient();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddDbContext<ApplicationDbContext>(DbContextSetup.Configure(hostContext.Configuration));

        #region Repositories
        services.AddScoped<IUnlockabledPhoneRepository, UnlockabledPhoneRepository>();
        services.AddScoped<IPhoneCarrierRepository, PhoneCarrierRepository>();
        services.AddScoped<IUnlockabledPhonePhoneUnlockToolRepository, UnlockabledPhonePhoneUnlockToolRepository>();
        services.AddScoped<IUnlockabledPhonePhoneCarrierRepository, UnlockabledPhonePhoneCarrierRepository>();
        #endregion Repositories

        #region Services
        services.AddScoped<IUnlockabledPhoneService, UnlockabledPhoneService>();
        services.AddScoped<IPhoneCarrierService, PhoneCarrierService>();
        services.AddScoped<IUnlockabledPhonePhoneUnlockToolService, UnlockabledPhonePhoneUnlockToolService>();
        services.AddScoped<IUnlockabledPhonePhoneCarrierService, UnlockabledPhonePhoneCarrierService>();
        services.AddScoped<ISamkeyDataSynchronizerService, SamkeyDataSynchronizerService>();
        services.AddScoped<ISamkeyFetchService, SamkeyFetchService >();
        services.AddScoped<ISamkeyPhoneProcessService, SamkeyPhoneProcessService >();
        services.AddScoped<IHttpService, HttpService>();

        #endregion Services

        #region Configure

        services.Configure<SamkeyUrlConfig>(hostContext.Configuration.GetSection("SamkeyUrlConfig"));

        #endregion Configure
    })
    .UseSerilog(SeriLogSetup.Configure)
    .Build();

await host.RunAsync();