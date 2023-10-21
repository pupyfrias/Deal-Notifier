using DealNotifier.Infrastructure.SamkeyDataSyncWorker.Interfaces;
using ILogger = Serilog.ILogger;

namespace DealNotifier.Infrastructure.SamkeyDataSyncWorker.Services
{
    public class SamkeyDataSynchronizerService : ISamkeyDataSynchronizerService
    {
        private readonly ILogger _logger;
        private readonly ISamkeyFetchService _samkeyFetchService;
        private readonly ISamkeyPhoneProcessService _samkeyPhoneProcessService;

        public SamkeyDataSynchronizerService(ILogger logger, ISamkeyFetchService samkeyFetchService, ISamkeyPhoneProcessService samkeyPhoneProcessService)
        {
            _logger = logger;
            _samkeyFetchService = samkeyFetchService;
            _samkeyPhoneProcessService = samkeyPhoneProcessService;

        }

        public async Task InitializeAsync()
        {
            try
            {
                var phoneModelList = await _samkeyFetchService.GetPhoneModelsAsync();

                if (phoneModelList != null)
                {
                    _logger.Information($"Processing {phoneModelList.Count} Phones");
                    await _samkeyPhoneProcessService.ProcessAsync(phoneModelList);
                }
                else
                {
                    _logger.Warning("Method GetPhoneModelsAsync returned a null");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }
    }
}