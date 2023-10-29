using Catalog.Application.Interfaces.Services;
using System.Net.Http.Headers;

namespace Catalog.Application.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<HttpResponseMessage> MakePostRequestAsync(string url, Dictionary<string, string> requestBody, Dictionary<string, string>? dataRequestHeader = null)
        {
            using (HttpRequestMessage requestMessage = new(HttpMethod.Post, url))
            {
                AddRequestHeaders(requestMessage.Headers, dataRequestHeader);
                HttpContent content = new FormUrlEncodedContent(requestBody);
                requestMessage.Content = content;
                return await _httpClient.SendAsync(requestMessage);
            }
        }

        public async Task<HttpResponseMessage> MakeGetRequestAsync(string url, Dictionary<string, string>? requestHeader = null)
        {
            using (HttpRequestMessage requestMessage = new(HttpMethod.Get, url))
            {
                AddRequestHeaders(requestMessage.Headers, requestHeader);
                return await _httpClient.SendAsync(requestMessage);
            }
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