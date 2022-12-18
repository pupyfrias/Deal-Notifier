using Microsoft.Extensions.Hosting;
using Serilog;

namespace WebScraping.Core.Application.SetupOptions
{
    public static class SeriLog
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Options = (hostBuilderContext, loggerContiguration) =>
        {
            loggerContiguration.ReadFrom
                .Configuration(hostBuilderContext.Configuration)
                .WriteTo.Console();
        };
    }
}
