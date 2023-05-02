﻿using Serilog;
using Serilog.Formatting;
using Serilog.Formatting.Json;
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
                 .Enrich.WithProperty("ApplicationName", "WebScraping")
                .CreateLogger();
            }

            return _logger;
        }
    }
}