using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting;
using Serilog.Formatting.Json;
using Serilog.Templates;
using WebScraping.Core.Application.Constants;

namespace WebScraping.Core.Application.SetupOptions
{
    public static class SeriLog
    {

        private static ITextFormatter consoleFormatter = new ExpressionTemplate(OutputTemplate.Console);

        public static readonly Action<HostBuilderContext, LoggerConfiguration> Options = (hostBuilderContext, loggerContiguration) =>
        {
            loggerContiguration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Serilog.AspNetCore.RequestLoggingMiddleware", Serilog.Events.LogEventLevel.Fatal)
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