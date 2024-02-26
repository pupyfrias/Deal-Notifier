using DealNotifier.Core.Application.Configs;
using DealNotifier.Infrastructure.GlobalUnlockerDataSyncWorker.Helpers;
using DealNotifier.Infrastructure.GlobalUnlockerDataSyncWorker.Interfaces;
using Microsoft.Extensions.Options;
using ILogger = Serilog.ILogger;

namespace DealNotifier.Infrastructure.GlobalUnlockerDataSyncWorker.Services
{
    public class GlobalUnlockerDataSynchronizerService : IGlobalUnlockerDataSynchronizerService
    {
        private readonly ILogger _logger;
        private readonly IGlobalUnlockerFetchService _globalUnlockerFetchService;
        private readonly IGlobalUnlockerPhoneProcessService _globalUnlockerPhoneProcessService;
        private readonly GlobalUnlockerUrlConfig _globalUnlockerUrlConfig;

        public GlobalUnlockerDataSynchronizerService(
            ILogger logger,
            IGlobalUnlockerFetchService fetchGlobalUnlocker,
            IGlobalUnlockerPhoneProcessService processPhoneGlobalUnlocker,
            IOptions<GlobalUnlockerUrlConfig> globalUnlockerUrlConfig

            )
        {
            _logger = logger;
            _globalUnlockerFetchService = fetchGlobalUnlocker;
            _globalUnlockerPhoneProcessService = processPhoneGlobalUnlocker;
            _globalUnlockerUrlConfig = globalUnlockerUrlConfig.Value;

        }

        public async Task InitializeAsync()
        {
            try
            {
                if (_globalUnlockerUrlConfig?.Paths == null)
                {
                    _logger.Warning("GlobalUnlocker URL config or paths are null. Initialization aborted.");
                    return;
                }

                foreach (string path in _globalUnlockerUrlConfig.Paths)
                {
                    try
                    {
                        var pageHtml = await _globalUnlockerFetchService.GetPageHTMLAsync(path);

                        if (pageHtml != null)
                        {
                            var brand = BrandHelper.GetBrandByName(path);
                            await _globalUnlockerPhoneProcessService.ProcessAsync(pageHtml, brand);
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