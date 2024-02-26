using DealNotifier.Infrastructure.GlobalUnlockerDataSyncWorker.Interfaces;
using ILogger = Serilog.ILogger;

namespace DealNotifier.Infrastructure.GlobalUnlockerDataSyncWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IGlobalUnlockerDataSynchronizerService _globalUnlockerDataSynchronizerService;

        public Worker(
            ILogger logger,
            IServiceScopeFactory serviceScopeFactory
            )
        {
            _logger = logger;
            var scope = serviceScopeFactory.CreateScope();
            _globalUnlockerDataSynchronizerService = scope.ServiceProvider.GetRequiredService<IGlobalUnlockerDataSynchronizerService>();
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.Information("Starting ExecuteAsync.");
                await _globalUnlockerDataSynchronizerService.InitializeAsync();
                _logger.Information("ExecuteAsync finished.");
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