using WorkerService.T_Unlock_WebScraping.Interfaces;
using ILogger = Serilog.ILogger;

namespace WorkerService.T_Unlock_WebScraping
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly ITUnlockDataSynchronizerService _tUnlockDataSynchronizerService;

        public Worker(
            ILogger logger,
            IServiceScopeFactory serviceScopeFactory
            )
        {
            _logger = logger;
            var scope = serviceScopeFactory.CreateScope();
            _tUnlockDataSynchronizerService = scope.ServiceProvider.GetRequiredService<ITUnlockDataSynchronizerService>();
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.Information("Starting ExecuteAsync.");
                await _tUnlockDataSynchronizerService.InitializeAsync();
                _logger.Information("ExecuteAsync finished.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
        }
    }
}