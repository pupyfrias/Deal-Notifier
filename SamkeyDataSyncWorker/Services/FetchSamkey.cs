using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Domain.Configs;
using Microsoft.Extensions.Options;
using SamkeyDataSyncWorker.Interfaces;
using SamkeyDataSyncWorker.ViewModels;
using ILogger = Serilog.ILogger;

namespace SamkeyDataSyncWorker.Services
{
    public class FetchSamkey : IFetchSamkey
    {
        private readonly ILogger _logger;
        private readonly SamkeyUrlConfig _samkeyUrlConfig;
        private readonly string _baseUrl;
        private readonly IHttpService _httpService;
        private readonly Dictionary<string, string> _headers;

        public FetchSamkey(ILogger logger, IOptions<SamkeyUrlConfig> samkeyUrlConfig, IHttpService httpService)
        {
            _logger = logger;
            _samkeyUrlConfig = samkeyUrlConfig.Value;
            _baseUrl = _samkeyUrlConfig.Base;
            _httpService = httpService;
            _headers = new Dictionary<string, string>
            {
                { "Origin", _baseUrl},
                { "Referer",  _baseUrl}
            };
        }
        public async Task<DetailsPhoneResponse?> GetDetailsPhoneAsync(string modelNumber)
        {

            string supportedPath = _samkeyUrlConfig.Paths.Supported;
            string url = _baseUrl + supportedPath;

            var requestBody = new Dictionary<string, string>
            {
                { "model", modelNumber },
                { "getsupported", "true" }
            };

            try
            {
                var response = await _httpService.MakePostRequestAsync<List<DetailsPhoneResponse>>(url, requestBody, _headers);
                return response?[0];
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }


        public async Task<List<string>?> GetPhoneModelsAsync()
        {
            string autoCompletePath = _samkeyUrlConfig.Paths.AutoComplete;
            string url = _baseUrl + autoCompletePath;
            var requestBody = new Dictionary<string, string> { { "query", "s" } };
            return await _httpService.MakePostRequestAsync<List<string>>(url, requestBody, _headers);
        }
    }
}
