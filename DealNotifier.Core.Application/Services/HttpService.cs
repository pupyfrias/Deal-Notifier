using DealNotifier.Core.Application.Interfaces.Services;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using System.Net;
using System.Net.Http.Headers;


namespace DealNotifier.Core.Application.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly IAsyncPolicy<HttpResponseMessage> _resiliencePolicy;
        private readonly ILogger _logger;

        public HttpService(IHttpClientFactory httpClientFactory, ILogger logger)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();

            var waitAndRetry = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(
                    5,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        _logger.Warning($"Retry {retryAttempt} due to {outcome.Exception?.Message ??
                            outcome.Result.StatusCode.ToString()}. Waiting {timespan}.");
                    });

            var circuitBreaker = HttpPolicyExtensions.HandleTransientHttpError()
                .CircuitBreakerAsync(
                    5,
                    TimeSpan.FromSeconds(30),
                    onBreak: (outcome, timespan, context) =>
                    {
                        _logger.Warning($"Circuit breaker opened due to {outcome.Exception?.Message ??
                            outcome.Result.StatusCode.ToString()}. Break time: {timespan}.");
                    },
                    onReset: context =>
                    {
                        _logger.Information("Circuit breaker reset.");
                    },
                    onHalfOpen: () =>
                    {
                        _logger.Information("Circuit breaker is half-open.");
                    });

            _resiliencePolicy = Policy.WrapAsync(waitAndRetry, circuitBreaker);
        }

        public async Task<HttpResponseMessage> MakePostRequestAsync(
            string url, Dictionary<string, 
            string> requestBody, 
            Dictionary<string, string>? dataRequestHeader = null
            )
        {
            return await _resiliencePolicy.ExecuteAsync(async () =>
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
            return await _resiliencePolicy.ExecuteAsync(async () =>
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