using WebScraping.Core.Application.Models;

namespace WebScraping.Core.Application.Heplers
{
    public class Helper
    {
        public static HashSet<string> checkedItemList = new HashSet<string>();
        public static List<SpBlackListResponse> linkBlackList = new List<SpBlackListResponse>();
        public static string basePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public static string GetLocalPath(string url)
        {
            Uri uri = new Uri(url);
            string newUrl = uri.Host+ uri.LocalPath;
            if (newUrl.Contains("/ref"))
                newUrl = newUrl.Substring(0, newUrl.IndexOf("/ref"));
            return newUrl;
        }
    }
}
