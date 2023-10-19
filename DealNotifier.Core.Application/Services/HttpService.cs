using DealNotifier.Core.Application.Interfaces.Services;
using Serilog;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DealNotifier.Core.Application.Services
{
    public class HttpService: IHttpService
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public HttpService(ILogger logger, IHttpClientFactory httpClientFactory)
        {           
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }


        public async Task<TDestination?> MakePostRequestAsync<TDestination>(string url, Dictionary<string, string> requestBody, Dictionary<string, string>? dataRequestHeader = null)
        {
            TDestination? responseBody = default;
            
            using (HttpRequestMessage requestMessage = new(HttpMethod.Post, url))
            {
                AddRequestHeaders(requestMessage.Headers, dataRequestHeader);
                HttpContent content = new FormUrlEncodedContent(requestBody);
                requestMessage.Content = content;
                HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadFromJsonAsync<TDestination>();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.Error($"{response.StatusCode} => {error}");
                }
            }
            return responseBody;
        }

        private void AddRequestHeaders(HttpRequestHeaders httpRequestHeaders, Dictionary<string, string>? dataHeader)
        {
            if (dataHeader != null)
            {
                foreach (var item in dataHeader)
                {
                    httpRequestHeaders.Add(item.Key, item.Value);
                }
            }
        }
    }
}
