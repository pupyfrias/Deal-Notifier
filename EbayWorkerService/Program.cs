using EbayWorkerService;
using Serilog;
using System.Reflection;
using WebScraping.Core.Application.Contracts.Services;
using WebScraping.Core.Application.SetupOptions;
using WebScraping.Infrastructure.Email;
using WebScraping.Infrastructure.Persistence.Models;
using WebScraping.Infrastructure.Persistence.Services;


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