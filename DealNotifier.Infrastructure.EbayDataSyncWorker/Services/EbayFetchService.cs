using DealNotifier.Core.Application.Configs;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.eBay;
using DealNotifier.Infrastructure.EbayDataSyncWorker.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using ILogger = Serilog.ILogger;

namespace DealNotifier.Infrastructure.EbayDataSyncWorker.Services
{
    public class EbayFetchService : IEbayFetchService
    {
        private readonly ILogger _logger;
        private readonly EbayUrlConfig _ebayUrlConfig;
        private readonly string _baseUrl;
        private readonly IHttpService _httpService;
        private readonly IMemoryCache _memoryCache;
        private int _retryFetch = 0;

        public EbayFetchService(
            ILogger logger,
            IOptions<EbayUrlConfig> ebayUrlConfig,
            IHttpService httpService,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _ebayUrlConfig = ebayUrlConfig.Value;
            _baseUrl = _ebayUrlConfig.Base;
            _httpService = httpService;
            _memoryCache = memoryCache;
        }

        private async Task<string> GetAccessToken()
        {
            _memoryCache.TryGetValue("accessToken", out string? accessToken);

            if (string.IsNullOrEmpty(accessToken))
            {
                await RefreshTokenAsync();
                _memoryCache.TryGetValue("accessToken", out accessToken);
            }

            return accessToken ?? string.Empty;
        }

        public async Task<EBayResponse?> GetItemsAsync(string url)
        {
            EBayResponse? ebayResponse = default;
            try
            {
                var accessToken = await GetAccessToken();
                var requestHeader = new Dictionary<string, string>() { { "Authorization", $"Bearer {accessToken}" } };

                var response = await _httpService.MakeGetRequestAsync(url, requestHeader);
                if (response.IsSuccessStatusCode)
                {
                    ebayResponse = await response.Content.ReadFromJsonAsync<EBayResponse>();
                    _retryFetch = 0;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (_retryFetch > 3)
                    {
                        _logger.Information("Maximum retry limit reached");
                        _retryFetch = 0;
                        return null;
                    }

                    _retryFetch++;
                    _logger.Information("Unauthorized");
                    await RefreshTokenAsync();
                    return await GetItemsAsync(url);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var content = await response.Content.ReadFromJsonAsync<EBayErrorResponse>();
                    var error = content?.Errors?.FirstOrDefault();
                    _logger.Warning($"Url: {url} Message: {error?.Message}");
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<EBayErrorResponse>();
                    var error = content?.Errors?.FirstOrDefault();
                    _logger.Error($"Error Trying consume eBay API. Message: {error?.Message} StatusCode: {(int)response.StatusCode}" +
                        $"Category: {error?.Category} Domain: {error?.Domain} ErrorId: {error?.ErrorId}");
                }
            }
            catch (Exception ex)
            {
                _logger?.Error($"An error occurred on method GetItemsAsync, Message: {ex.Message}. " +
                    $"InnerException: {ex.InnerException?.Message}");
            }

            return ebayResponse;
        }

        private async Task RefreshTokenAsync()
        {
            string clientId = Environment.GetEnvironmentVariable("ClientId") ?? string.Empty;
            string clientSecret = Environment.GetEnvironmentVariable("ClientSecret") ?? string.Empty;
            string refreshToken = Environment.GetEnvironmentVariable("RefreshToken") ?? string.Empty;
            string scope = _ebayUrlConfig.Scope;

            string tokenUrl = _baseUrl + _ebayUrlConfig.Paths.Token;
            string credentials = $"{clientId}:{clientSecret}";
            string base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));

            var requestBody = new Dictionary<string, string>()
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken },
                {"scope", scope }
            };

            var requestHeader = new Dictionary<string, string>() { { "Authorization", $"Basic {base64Credentials}" } };

            HttpResponseMessage response = await _httpService.MakePostRequestAsync(tokenUrl, requestBody, requestHeader);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<EbayLoginResponse>();
                _memoryCache.Set("accessToken", data!.AccessToken);
                _logger.Information("Token Refreshed");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.Error($"Error Trying refresh toke. Message: {error}");
            }
        }
    }
}