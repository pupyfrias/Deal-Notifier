using EbayWorkerService;
using Serilog;
using System.Reflection;
using WebScraping.Core.Application.Interfaces.Services;
using WebScraping.Core.Application.SetupOptions;
using WebScraping.Infrastructure.Persistence.Models;
using WebScraping.Infrastructure.Persistence.Services;


Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

IHost host = Host.CreateDefaultBuilder(args)
        
    .UseWindowsService(options =>
    {
        options.ServiceName = "ebay";
       
    })
    .UseSerilog(SeriLog.Options)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IItemService, ItemService>();
        services.AddSingleton<IEbayService, EbayService>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    })
   .Build();

await host.RunAsync();
