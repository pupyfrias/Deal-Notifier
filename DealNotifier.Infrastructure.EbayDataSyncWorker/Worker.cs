using DealNotifier.Infrastructure.EbayDataSyncWorker.Interfaces;
using System.Diagnostics;
using ILogger = Serilog.ILogger;

namespace DealNotifier.Infrastructure.EbayDataSyncWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IEbayDataSynchronizerService _ebayDataSynchronizerService;

        public Worker(ILogger logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            var scope = serviceScopeFactory.CreateScope();
            _ebayDataSynchronizerService = scope.ServiceProvider.GetRequiredService<IEbayDataSynchronizerService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.Information($"EbayDataSyncService Initialized.");
                var timer = new Stopwatch();
                timer.Start();
                await _ebayDataSynchronizerService.InitializeAsync();
                timer.Stop();
                TimeSpan timeTaken = timer.Elapsed;

                _logger.Information($"Time taken: {timeTaken.ToString(@"m\:ss\.fff")}");
                _logger.Information("EbayDataSyncService completed.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
            finally
            {
                Environment.Exit(0);
            }
        }
    }
}