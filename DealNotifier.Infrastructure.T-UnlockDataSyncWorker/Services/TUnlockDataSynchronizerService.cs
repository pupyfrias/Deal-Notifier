using DealNotifier.Core.Application.Configs;
using Microsoft.Extensions.Options;
using WorkerService.T_Unlock_WebScraping.Helpers;
using WorkerService.T_Unlock_WebScraping.Interfaces;
using ILogger = Serilog.ILogger;

namespace WorkerService.T_Unlock_WebScraping.Services
{
    public class TUnlockDataSynchronizerService : ITUnlockDataSynchronizerService
    {
        private readonly ILogger _logger;
        private readonly ITUnlockFetchService _tUnlockFetchService;
        private readonly ITUnlockPhoneProcessService _tUnlockPhoneProcessService;
        private readonly GlobalUnlockerUrlConfig _tUnlockUrlConfig;

        public TUnlockDataSynchronizerService(
            ILogger logger, 
            ITUnlockFetchService fetchTUnlock, 
            ITUnlockPhoneProcessService processPhoneTUnlock,
            IOptions<GlobalUnlockerUrlConfig> tUnlockUrlConfig
            
            )
        {
            _logger = logger;
            _tUnlockFetchService = fetchTUnlock;
            _tUnlockPhoneProcessService = processPhoneTUnlock;
            _tUnlockUrlConfig = tUnlockUrlConfig.Value;

        }

        public async Task InitializeAsync()
        {
            try
            {
                if (_tUnlockUrlConfig?.Paths == null)
                {
                    _logger.Warning("TUnlock URL config or paths are null. Initialization aborted.");
                    return;
                }

                foreach (string path in _tUnlockUrlConfig.Paths)
                {
                    try
                    {
                        var pageHtml = await _tUnlockFetchService.GetPageHTMLAsync(path);

                        if (pageHtml != null)
                        {
                            var brand = BrandHelper.GetBrandByName(path);
                            await _tUnlockPhoneProcessService.ProcessAsync(pageHtml, brand);
                        }
                        else
                        {
                            _logger.Warning($"Page HTML for path {path} is null.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"An error occurred while processing path {path}: {ex.Message}", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred during initialization: {ex.Message}", ex);
            }
        }
    }
}