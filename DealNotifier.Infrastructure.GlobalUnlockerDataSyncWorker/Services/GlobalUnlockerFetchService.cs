using DealNotifier.Core.Application.Configs;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Infrastructure.GlobalUnlockerDataSyncWorker.Interfaces;
using Microsoft.Extensions.Options;
using ILogger = Serilog.ILogger;

namespace DealNotifier.Infrastructure.GlobalUnlockerDataSyncWorker.Services
{
    public class GlobalUnlockerFetchService : IGlobalUnlockerFetchService
    {
        private readonly ILogger _logger;

        private readonly string _baseUrl;
        private readonly IHttpService _httpService;


        public GlobalUnlockerFetchService(ILogger logger, IOptions<GlobalUnlockerUrlConfig> globalUnlockerUrlConfig, IHttpService httpService)
        {
            _logger = logger;
            _baseUrl = globalUnlockerUrlConfig.Value.Base;
            _httpService = httpService;
        }


        public async Task<string?> GetPageHTMLAsync(string path)
        {
            string url = _baseUrl + path;
            _logger.Information($"Getting Page HTML of {url}");
            var response = await _httpService.MakeGetRequestAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
