using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Domain.Entities;
using System.Collections.Concurrent;

namespace DealNotifier.Core.Application.Helpers
{
    public static class Helper
    {


        public static string BasePath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        /// <summary>
        /// Get URL local path
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Url local path</returns>
        public static string GetLocalPath(string url)
        {
            Uri uri = new Uri(url);
            string newUrl = $"{uri.Scheme}://{uri.Host}{uri.LocalPath}";
            if (newUrl.Contains("/ref"))
                newUrl = newUrl.Substring(0, newUrl.IndexOf("/ref"));
            return newUrl;
        }
    }
}