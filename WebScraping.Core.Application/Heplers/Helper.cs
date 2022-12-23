using WebScraping.Core.Application.DTOs;

namespace WebScraping.Core.Application.Heplers
{
    public class Helper
    {
        public static List<string> checkedItemList = new List<string>();
        public static List<BlackListDTO> linkBlackList = new List<BlackListDTO>();
        public static string basePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static string appsettingsPath = Path.GetFullPath("..\\WebScraping.Core.Application\\appsettings.json");

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