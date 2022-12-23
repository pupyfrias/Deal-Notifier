using System.Text.RegularExpressions;

namespace WebScraping.Core.Application.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Remove all spacial Characters from the string
        /// </summary>
        /// <param name="str"></param>
        /// <returns>cleaned string</returns>
        public static string RemoveSpecialCharacters(this string str)
        {
            string newString = Regex.Replace(str, "[,.+'\":;]", "", RegexOptions.Compiled);
            return newString.Replace("BRAND NEW", "").Replace("NEW LISTING", "");
        }
    }
}