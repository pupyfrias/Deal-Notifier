using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.Services;
using Catalog.Domain.Configs;
using Catalog.Persistence.DbContexts;
using Catalog.Persistence.Repositories;
using Catalog.Persistence.Setup;
using Identity.Application.SetupOptions;
using Serilog;
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
        services.AddScoped<ITUnlockDataSynchronizerService, TUnlockDataSynchronizerService>();
        services.AddScoped<TUnlockFetchService, TUnlockFetchService>();
        services.AddScoped<IHttpService, HttpService>();
        services.AddScoped<IPhoneCarrierService, PhoneCarrierService>();
        services.AddScoped<ITUnlockPhoneProcessService, TUnlockPhoneProcessService>();
        services.AddScoped<IUnlockabledPhonePhoneCarrierService, UnlockabledPhonePhoneCarrierService>();
        services.AddScoped<IUnlockabledPhonePhoneUnlockToolService, UnlockabledPhonePhoneUnlockToolService>();
        services.AddScoped<IUnlockabledPhoneService, UnlockabledPhoneService>();
        services.AddScoped<ITUnlockFetchService, TUnlockFetchService>();

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
    //Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
}
await host.RunAsync();