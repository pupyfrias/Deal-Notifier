using Microsoft.Extensions.Logging;

namespace WebScraping.Utils
{
    public class LogConsole
    {
        /// <summary>
        /// Create a Logger
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>ILogger</returns>
        public static ILogger CreateLogger<T>() where T : class
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddConsole();
            });

            return loggerFactory.CreateLogger<T>();
        }
    }
}
