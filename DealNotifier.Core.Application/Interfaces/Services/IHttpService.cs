namespace Catalog.Application.Interfaces.Services
{
    public interface IHttpService
    {
        Task<HttpResponseMessage> MakeGetRequestAsync(string url, Dictionary<string, string>? requestHeader = null);
        Task<HttpResponseMessage> MakePostRequestAsync(string url, Dictionary<string, string> requestBody, Dictionary<string, string>? dataRequestHeader = null);
    }
}