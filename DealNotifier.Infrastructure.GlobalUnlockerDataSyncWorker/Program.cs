using DealNotifier.Core.Application.Configs;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.Setups;
using DealNotifier.Infrastructure.GlobalUnlockerDataSyncWorker;
using DealNotifier.Infrastructure.GlobalUnlockerDataSyncWorker.Interfaces;
using DealNotifier.Infrastructure.GlobalUnlockerDataSyncWorker.Services;
using DealNotifier.Persistence.DbContexts;
using DealNotifier.Persistence.Repositories;
using DealNotifier.Persistence.Setup;
using Serilog;
using System.Reflection;

IHost host = Host.CreateDefaultBuilder(args)

    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddHttpContextAccessor();
        services.AddHttpClient();
        services.AddMemoryCache();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddDbContext<ApplicationDbContext>(DbContextSetup.Configure(hostContext.Configuration), ServiceLifetime.Transient);


        #region Repositories
        services.AddScoped<IUnlockabledPhoneRepository, UnlockabledPhoneRepository>();
        services.AddScoped<IPhoneCarrierRepository, PhoneCarrierRepository>();
        services.AddScoped<IUnlockabledPhonePhoneUnlockToolRepository, UnlockabledPhonePhoneUnlockToolRepository>();
        services.AddScoped<IUnlockabledPhonePhoneCarrierRepository, UnlockabledPhonePhoneCarrierRepository>();

        #endregion Repositories

        #region Services
        services.AddScoped<IGlobalUnlockerDataSynchronizerService, GlobalUnlockerDataSynchronizerService>();
        services.AddScoped<GlobalUnlockerFetchService, GlobalUnlockerFetchService>();
        services.AddScoped<IHttpService, HttpService>();
        services.AddScoped<IPhoneCarrierService, PhoneCarrierService>();
        services.AddScoped<IGlobalUnlockerPhoneProcessService, GlobalUnlockerPhoneProcessService>();
        services.AddScoped<IUnlockabledPhonePhoneCarrierService, UnlockabledPhonePhoneCarrierService>();
        services.AddScoped<IUnlockabledPhonePhoneUnlockToolService, UnlockabledPhonePhoneUnlockToolService>();
        services.AddScoped<IUnlockabledPhoneService, UnlockabledPhoneService>();
        services.AddScoped<IGlobalUnlockerFetchService, GlobalUnlockerFetchService>();

        #endregion Services

        #region Configure

        services.Configure<GlobalUnlockerUrlConfig>(hostContext.Configuration.GetSection("GlobalUnlockerUrlConfig"));

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