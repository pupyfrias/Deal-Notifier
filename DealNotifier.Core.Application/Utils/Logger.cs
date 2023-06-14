using DealNotifier.Core.Application.Constants;
using DealNotifier.Core.Application.Heplers;
using Serilog;
using Serilog.Formatting;
using Serilog.Formatting.Json;
using Serilog.Templates;

namespace DealNotifier.Core.Application.Utils
{
    public class Logger
    {
        private static ILogger? _logger;
        private static ITextFormatter consoleFormatter = new ExpressionTemplate(OutputTemplate.Console);

        /// <summary>
        /// Create a Logger
        /// </summary>
        /// <returns>ILogger</returns>
        public static ILogger CreateLogger()
        {
            if (_logger is null)
            {
                _logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    formatter: consoleFormatter)
                .WriteTo.Logger(options =>
                {
                    options.Filter.ByIncludingOnly(filterOptions =>
                    {
                        return filterOptions.Level == Serilog.Events.LogEventLevel.Information;
                    })
                    .WriteTo.File(
                        path: $"{Helper.BasePath}/Logs/Info/log-.json",
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
                         path: $"{Helper.BasePath}/Logs/Errors/error-.json",
                         rollingInterval: RollingInterval.Day,
                         retainedFileCountLimit: 7,
                         formatter: new JsonFormatter()
                     );
                 })
                 .Enrich.FromLogContext()
                 .Enrich.WithProperty("ApplicationName", "DealNotifier")
                .CreateLogger();
            }

            return _logger;
        }
    }
}