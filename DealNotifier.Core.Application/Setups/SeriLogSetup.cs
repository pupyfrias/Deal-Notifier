using DealNotifier.Core.Application.Constants;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting;
using Serilog.Formatting.Json;
using Serilog.Templates;

namespace DealNotifier.Core.Application.Setups
{
    public static class SeriLogSetup
    {
        private static ITextFormatter consoleFormatter = new ExpressionTemplate(OutputTemplate.Console);

        public static readonly Action<HostBuilderContext, LoggerConfiguration> Configure = (hostBuilderContext, loggerContiguration) =>
        {
            loggerContiguration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Serilog.AspNetCore.RequestLoggingMiddleware", Serilog.Events.LogEventLevel.Fatal)
            .MinimumLevel.Override("System.Net.Http.HttpClient", Serilog.Events.LogEventLevel.Warning)
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
            .WriteTo.Console(formatter: consoleFormatter)
            .Enrich.FromLogContext();
        };
    }
}