using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraping.Heplers
{
    public class Helper
    {
        public static HashSet<string> checkList = new HashSet<string>();
        public static List<string> linkBlackList = new List<string>();
        public static string basePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    }
}
