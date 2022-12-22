using Microsoft.Extensions.Hosting;
using Serilog;

namespace WebScraping.Core.Application.SetupOptions
{
    public static class SeriLog
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Options = (hostBuilderContext, loggerContiguration) =>
        {
            loggerContiguration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .WriteTo.File(
                path: "./logs/log-.txt", 
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true
                )
            .WriteTo.Console()
            .Enrich.FromLogContext();

        };

    }
}
