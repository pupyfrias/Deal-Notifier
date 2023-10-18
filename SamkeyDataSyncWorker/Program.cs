using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.Setups;
using DealNotifier.Core.Domain.Configs;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using DealNotifier.Infrastructure.Persistence.Repositories;
using DealNotifier.Infrastructure.Persistence.Setup;
using SamkeyDataSyncWorker;
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
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddDbContext<ApplicationDbContext>(DbContextSetup.Configure(hostContext.Configuration));


        #region Repositories

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnlockabledPhoneRepository, UnlockabledPhoneRepository>();
        services.AddScoped<IPhoneCarrierRepository, PhoneCarrierRepository>();
        services.AddScoped<IUnlockabledPhonePhoneUnlockToolRepository, UnlockabledPhonePhoneUnlockToolRepository>();
        services.AddScoped<IUnlockabledPhonePhoneCarrierRepository, UnlockabledPhonePhoneCarrierRepository>();

        #endregion Repositories

        #region Services

        services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
        services.AddScoped<IUnlockabledPhoneService, UnlockabledPhoneService>();
        services.AddScoped<IPhoneCarrierService, PhoneCarrierService>();
        services.AddScoped<IUnlockabledPhonePhoneUnlockToolService, UnlockabledPhonePhoneUnlockToolService>();
        services.AddScoped<IUnlockabledPhonePhoneCarrierService, UnlockabledPhonePhoneCarrierService>();

        #endregion Services

        #region Configure

        services.Configure<SamkeyUrlConfig>(hostContext.Configuration.GetSection("SamkeyUrlConfig"));

        #endregion Configure
    })
    .UseSerilog(SeriLogSetup.Configure)
    .Build();

await host.RunAsync();