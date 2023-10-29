using Microsoft.AspNetCore.Http;

namespace Identity.Application.Extensions
{
    public static class HttpRequestExtension
    {
        public static string GetBaseUrl(this HttpRequest httpRequest)
        {
            var scheme = httpRequest.Scheme;
            var host = httpRequest.Host;
            var path = httpRequest.Path;
            return $"{scheme}://{host}{path}";

        }
    }
}