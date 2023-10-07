using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.Setups;
using DealNotifier.Infrastructure.Email;
using DealNotifier.Infrastructure.Persistence.Models;
using EbayWorkerService;
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
        options.ServiceName = "ebay";
    })
    .UseSerilog(SeriLogSetup.Configure)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IItemServiceAsync, ItemServiceAsync>();
        services.AddSingleton<IEbayService, EbayService>();
        services.AddInfrastructureEmail(hostContext.Configuration);
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    })
   .Build();

await host.RunAsync();