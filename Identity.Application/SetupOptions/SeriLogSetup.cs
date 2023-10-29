using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;

namespace Identity.Application.SetupOptions
{
    public static class SeriLogSetup
    {
        public static readonly Action<HostBuilderContext, LoggerConfiguration> Configure = (hostBuilderContext, loggerContiguration) =>
        {
            loggerContiguration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Serilog.AspNetCore.RequestLoggingMiddleware", Serilog.Events.LogEventLevel.Information)
            .WriteTo.Logger(options =>
            {
                options.Filter.ByIncludingOnly(filterOptions =>
                {
                    return filterOptions.Level == Serilog.Events.LogEventLevel.Information;
                })
                .WriteTo.File(
                    path: "./Logs/Info/log-.json",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    formatter: new JsonFormatter()
                );
            })
             .WriteTo.Logger(options =>
             {
                 options.Filter.ByIncludingOnly(filterOptions =>
                 {
                     return filterOptions.Level == Serilog.Events.LogEventLevel.Error;
                 })
                .WriteTo.File(
                     path: "./Logs/Errors/error-.json",
                     rollingInterval: RollingInterval.Day,
                     retainedFileCountLimit: 7,
                     formatter: new JsonFormatter()
                 );
             })
            .WriteTo.Console(new CompactJsonFormatter())
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", "DealNotifierSecurity")
            .Enrich.WithMachineName();
        };
    }
}