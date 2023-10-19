namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IHttpService
    {
       Task<TDestination?> MakePostRequestAsync<TDestination>(string url, Dictionary<string, string> requestBody, Dictionary<string, string>? dataRequestHeader = null);
    }
}