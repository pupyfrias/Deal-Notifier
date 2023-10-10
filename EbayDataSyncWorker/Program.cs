using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.Setups;
using DealNotifier.Infrastructure.Email;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using DealNotifier.Infrastructure.Persistence.Models;
using DealNotifier.Infrastructure.Persistence.Repositories;
using EbayDataSyncWorker;
using Serilog;
using System.Reflection;

var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

if (environment != "Development")
{
    Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
}

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "eBayDataSync";
    })
    .UseSerilog(SeriLogSetup.Configure)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddInfrastructureEmail(hostContext.Configuration);
        services.AddSingleton<IEbayService, EbayService>();
        services.AddSingleton<IItemSyncService, ItemSyncService>();
        services.AddSingleton<IBanKeywordRepository, BanKeywordRepository>();
    })
   .Build();

await host.RunAsync();