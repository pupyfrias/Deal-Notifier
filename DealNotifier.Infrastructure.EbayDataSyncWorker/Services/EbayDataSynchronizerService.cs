using DealNotifier.Infrastructure.EbayDataSyncWorker.Interfaces;
using ILogger = Serilog.ILogger;

namespace DealNotifier.Infrastructure.EbayDataSyncWorker.Services
{
    public class EbayDataSynchronizerService : IEbayDataSynchronizerService
    {
        private readonly ILogger _logger;
        private readonly IEbayPhoneProcessService _ebayPhoneProcessService;

        public EbayDataSynchronizerService(ILogger logger, IEbayPhoneProcessService ebayPhoneProcessService)
        {
            _logger = logger;
            _ebayPhoneProcessService = ebayPhoneProcessService;
        }

        public async Task InitializeAsync()
        {
            try
            {
                await _ebayPhoneProcessService.ProcessAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }
    }
}