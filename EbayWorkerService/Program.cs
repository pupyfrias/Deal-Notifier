using EbayWorkerService;
using Serilog;
using System.Reflection;
using DealNotifier.Core.Application.Contracts.Services;
using DealNotifier.Core.Application.SetupOptions;
using DealNotifier.Infrastructure.Email;
using DealNotifier.Infrastructure.Persistence.Models;
using DealNotifier.Infrastructure.Persistence.Services;


var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

if(environment != "Development")
{
    Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
}


IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "ebay";
    })
    .UseSerilog(SeriLog.Options)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IItemService, ItemService>();
        services.AddSingleton<IEbayService, EbayService>();
        services.AddInfrastructureEmail(hostContext.Configuration);
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    })
   .Build();


await host.RunAsync();