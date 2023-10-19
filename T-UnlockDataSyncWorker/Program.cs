using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.Setups;
using DealNotifier.Core.Domain.Configs;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using DealNotifier.Infrastructure.Persistence.Repositories;
using DealNotifier.Infrastructure.Persistence.Setup;
using Serilog;
using System.Reflection;
using WorkerService.T_Unlock_WebScraping;
using WorkerService.T_Unlock_WebScraping.Interfaces;
using WorkerService.T_Unlock_WebScraping.Services;

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
        services.AddScoped<IDataSynchronizerTUnlockService, DataSynchronizerTUnlockService>();
        services.AddScoped<IFetchTUnlock, FetchTUnlock>();
        services.AddScoped<IHttpService, HttpService>();
        services.AddScoped<IPhoneCarrierService, PhoneCarrierService>();
        services.AddScoped<IProcessPhoneTUnlock, ProcessPhoneTUnlock>();
        services.AddScoped<IUnlockabledPhonePhoneCarrierService, UnlockabledPhonePhoneCarrierService>();
        services.AddScoped<IUnlockabledPhonePhoneCarrierTUnlock, UnlockabledPhonePhoneCarrierTUnlock>();
        services.AddScoped<IUnlockabledPhonePhoneUnlockToolService, UnlockabledPhonePhoneUnlockToolService>();
        services.AddScoped<IUnlockabledPhoneService, UnlockabledPhoneService>();

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