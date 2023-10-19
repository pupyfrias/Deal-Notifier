using SamkeyDataSyncWorker.Interfaces;
using ILogger = Serilog.ILogger;

namespace SamkeyDataSyncWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IDataSynchronizerSamkeyService _dataSynchronizerSamkeyService;


        public Worker(ILogger logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            var scope = serviceScopeFactory.CreateScope();
            _dataSynchronizerSamkeyService = scope.ServiceProvider.GetRequiredService<IDataSynchronizerSamkeyService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Starting ExecuteAsync.");
            await _dataSynchronizerSamkeyService.InitializeAsync();
            _logger.Information("ExecuteAsync finished.");
        }         
    }
}