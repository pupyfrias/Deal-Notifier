using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Domain.Configs;
using Microsoft.Extensions.Options;
using WorkerService.T_Unlock_WebScraping.Interfaces;
using ILogger = Serilog.ILogger;

namespace WorkerService.T_Unlock_WebScraping.Services
{
    public class TUnlockFetchService : ITUnlockFetchService
    {
        private readonly ILogger _logger;

        private readonly string _baseUrl;
        private readonly IHttpService _httpService;
  

        public TUnlockFetchService(ILogger logger, IOptions<TUnlockUrlConfig> tUnlockUrlConfig, IHttpService httpService)
        {
            _logger = logger;
            _baseUrl = tUnlockUrlConfig.Value.Base;
            _httpService = httpService;   
        }
       

        public async Task<string?> GetPageHTMLAsync(string path)
        {
            string url = _baseUrl + path;
            _logger.Information( $"Getting Page HTML of {url}");
            var response = await _httpService.MakeGetRequestAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
