using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Domain.Configs;
using DealNotifier.Infrastructure.SamkeyDataSyncWorker.Interfaces;
using DealNotifier.Infrastructure.SamkeyDataSyncWorker.ViewModels;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using ILogger = Serilog.ILogger;

namespace DealNotifier.Infrastructure.SamkeyDataSyncWorker.Services
{
    public class SamkeyFetchService : ISamkeyFetchService
    {
        private readonly ILogger _logger;
        private readonly SamkeyUrlConfig _samkeyUrlConfig;
        private readonly string _baseUrl;
        private readonly IHttpService _httpService;
        private readonly Dictionary<string, string> _headers;

        public SamkeyFetchService(ILogger logger, IOptions<SamkeyUrlConfig> samkeyUrlConfig, IHttpService httpService)
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
        public async Task<PhoneDetailsResponse?> GetPhoneDetailsAsync(string modelNumber)
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
                var response = await _httpService.MakePostRequestAsync(url, requestBody, _headers);
                var phoneDetailsResponse = await response.Content.ReadFromJsonAsync<IEnumerable<PhoneDetailsResponse>>();
                return phoneDetailsResponse?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Warning($"Exception occurred making POST request to Samkey. Url: {url}, ModelNumber: {modelNumber}, " +
                    $"Exception: {ex.Message}, InnerException: {ex.InnerException?.Message}");
                return null;
            }
        }


        public async Task<List<string>?> GetPhoneModelsAsync()
        {
            string autoCompletePath = _samkeyUrlConfig.Paths.AutoComplete;
            string url = _baseUrl + autoCompletePath;
            var requestBody = new Dictionary<string, string> { { "query", "s" } };
            var response = await _httpService.MakePostRequestAsync(url, requestBody, _headers);

            return await response.Content.ReadFromJsonAsync<List<string>>();
        }
    }
}
