using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.Setups;
using DealNotifier.Core.Domain.Configs;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using DealNotifier.Infrastructure.Persistence.Repositories;
using Serilog;
using System.Reflection;
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

        services.AddSingleton(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddSingleton<IUnlockableRepository, UnlockableRepository>();
        services.AddSingleton<IPhoneCarrierRepository, PhoneCarrierRepository>();
        services.AddSingleton<IUnlockabledPhonePhoneUnlockToolRepository, UnlockabledPhonePhoneUnlockToolRepository>();
        services.AddSingleton<IUnlockabledPhonePhoneCarrierRepository, UnlockabledPhonePhoneCarrierRepository>();

        #endregion Repositories

        #region Services

        services.AddSingleton(typeof(IGenericService<,>), typeof(GenericService<,>));
        services.AddSingleton<IUnlockableService, UnlockableService>();
        services.AddSingleton<IPhoneCarrierService, PhoneCarrierService>();
        services.AddSingleton<IUnlockabledPhonePhoneUnlockToolService, UnlockabledPhonePhoneUnlockToolService>();
        services.AddSingleton<IUnlockabledPhonePhoneCarrierService, UnlockabledPhonePhoneCarrierService>();

        #endregion Services

        #region Configure

        services.Configure<SamkeyUrlConfig>(hostContext.Configuration.GetSection("SamkeyUrlConfig"));

        #endregion Configure
    })
    .UseSerilog(SeriLogSetup.Configure)
    .Build();

await host.RunAsync();