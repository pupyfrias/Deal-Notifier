using DealNotifier.Core.Application.Interfaces.Services;
using ILogger = Serilog.ILogger;
using Timer = System.Threading.Timer;

namespace EbayWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly IEbayService _ebayService;
        private readonly ILogger _logger;
        private Timer _timer;

        public Worker(ILogger logger, IEbayService ebayService)
        {
            _logger = logger;
            _ebayService = ebayService;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Starting ExecuteAsync.");
            TimerCallback callback = async (state) => await TimerElapsed();

            _timer = new Timer(callback, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            await Task.Delay(-1, stoppingToken);
            _logger.Information("ExecuteAsync finished.");
        }

        private async Task TimerElapsed()
        {
            _logger.Information($"Timer elapsed. Running eBay Service Init. {DateTime.Now}");
            await _ebayService.Init();
            _logger.Information("eBay Service Init completed.");
        }
    }
}