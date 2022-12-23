using Serilog;
using Serilog.Formatting;
using Serilog.Templates;
using WebScraping.Core.Application.Constants;
using WebScraping.Core.Application.Heplers;

namespace WebScraping.Core.Application.Utils
{
    public class Logger
    {
        private static ILogger? _logger;
        private static ITextFormatter consoleFormatter = new ExpressionTemplate(OutputTemplate.Console);
        private static ITextFormatter fileFormatter = new ExpressionTemplate(OutputTemplate.File);

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
                .WriteTo.File(path: $"{Helper.basePath}/logs/log-.json",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning,
                    formatter: fileFormatter)
                .CreateLogger();
            }

            return _logger;
        }
    }
}