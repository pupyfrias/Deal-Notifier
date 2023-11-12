using DealNotifier.Core.Application.Interfaces.Services;
using Polly;
using Serilog;
using System.Net;
using System.Net.Http.Headers;


namespace DealNotifier.Core.Application.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;
        private readonly ILogger _logger;

        public HttpService(IHttpClientFactory httpClientFactory, ILogger logger)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _retryPolicy = Policy
                           .HandleResult<HttpResponseMessage>(response => response.StatusCode >= HttpStatusCode.InternalServerError)
                           .Or<HttpRequestException>()
                           .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                            onRetry: (outcome, timespan, retryAttempt, context) =>
                            {
                                _logger.Warning($"Retry {retryAttempt} for error: {outcome.Result.StatusCode}. waiting {timespan}.");
                            });
        }

        public async Task<HttpResponseMessage> MakePostRequestAsync(string url, Dictionary<string, string> requestBody, Dictionary<string, string>? dataRequestHeader = null)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                using (HttpRequestMessage requestMessage = new(HttpMethod.Post, url))
                {
                    AddRequestHeaders(requestMessage.Headers, dataRequestHeader);
                    HttpContent content = new FormUrlEncodedContent(requestBody);
                    requestMessage.Content = content;
                    return await _httpClient.SendAsync(requestMessage);
                }

            });

            
        }

        public async Task<HttpResponseMessage> MakeGetRequestAsync(string url, Dictionary<string, string>? requestHeader = null)
        {
            return await _retryPolicy.ExecuteAsync(async () => 
            {
                using (HttpRequestMessage requestMessage = new(HttpMethod.Get, url))
                {
                    AddRequestHeaders(requestMessage.Headers, requestHeader);
                    return await _httpClient.SendAsync(requestMessage);
                }
            });
        }

        private void AddRequestHeaders(HttpRequestHeaders httpRequestHeaders, Dictionary<string, string>? headerData)
        {
            if (headerData != null)
            {
                foreach (var item in headerData)
                {
                    httpRequestHeaders.Add(item.Key, item.Value);
                }
            }
        }
    }
}