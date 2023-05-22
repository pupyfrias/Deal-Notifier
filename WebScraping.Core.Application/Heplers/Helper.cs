using System.Collections.Concurrent;
using WebScraping.Core.Application.Dtos;
using WebScraping.Core.Application.Dtos.PhoneCarrier;
using WebScraping.Core.Application.DTOs;

namespace WebScraping.Core.Application.Heplers
{
    public static class Helper
    {
        public static string AppsettingsPath { get; } = Path.GetFullPath("..\\WebScraping.Core.Application\\appsettings.json");
        public static HashSet<BannedDto> BannedKeywordList { get; set; } = new HashSet<BannedDto>();
        public static HashSet<ConditionsToNotifyDto> ConditionsToNotifyList { get; set; } = new HashSet<ConditionsToNotifyDto>();
        public static HashSet<BrandReadDto> BrandList { get; set; } = new HashSet<BrandReadDto>();
        public static HashSet<PhoneCarrierReadDto> PhoneCarrierList { get; set; } = new HashSet<PhoneCarrierReadDto>();
        public static string BasePath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static HashSet<BlackListDto> BlacklistedLinks { get; set; } = new HashSet<BlackListDto>();
        public static ConcurrentBag<string> CheckedList { get; set; } = new ConcurrentBag<string>();

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