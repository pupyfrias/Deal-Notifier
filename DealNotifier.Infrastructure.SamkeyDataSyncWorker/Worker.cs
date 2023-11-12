using DealNotifier.Infrastructure.SamkeyDataSyncWorker.Interfaces;
using ILogger = Serilog.ILogger;

namespace DealNotifier.Infrastructure.SamkeyDataSyncWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly ISamkeyDataSynchronizerService _samkeyDataSynchronizerService;


        public Worker(ILogger logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            var scope = serviceScopeFactory.CreateScope();
            _samkeyDataSynchronizerService = scope.ServiceProvider.GetRequiredService<ISamkeyDataSynchronizerService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.Information("Starting ExecuteAsync.");
                await _samkeyDataSynchronizerService.InitializeAsync();
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