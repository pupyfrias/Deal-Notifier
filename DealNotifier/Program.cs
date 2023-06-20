using DealNotifier.Core.Application.Utils;
using Serilog;
using System.Diagnostics;

namespace WebScraping
{
    public class Program
    {
        private static ILogger _logger;

        private static async Task Main(string[] args)
        {
            _logger = Logger.CreateLogger().ForContext<Program>();

            try
            {
                /*                var eBayService = new EbayService();

                                await Timer(eBayService.Init);*/
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
            }
            finally
            {
                //await Timer(UpdateItems);
            }
        }

        private static void Timer(Action action)
        {
            var timer = new Stopwatch();
            timer.Start();
            action();
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            string time = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
            _logger.Information(time);
        }

        private static async Task Timer(Func<Task> action)
        {
            var timer = new Stopwatch();
            timer.Start();
            await action();
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            string time = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
            _logger.Information(time);
        }
    }
}