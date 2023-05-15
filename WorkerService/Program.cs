using Serilog;
using Serilog.Events;
using System.Reflection;
using WorkerService;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.File("./logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();

try
{
    Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    IHost host = Host.CreateDefaultBuilder(args)
        .UseWindowsService(options =>
        {
            options.ServiceName = "Razer Bulk Service";
        })
        .ConfigureAppConfiguration((hostContext, configBuilder) =>
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            configBuilder.
            AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        })
        .UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext())
        .ConfigureServices((hostContext, services) =>
        {
            services.AddHostedService<Worker>();
        })
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "There was a problem starting the service");
}
finally
{
    Log.Information("Service successfully stopped");

    Log.CloseAndFlush();
}