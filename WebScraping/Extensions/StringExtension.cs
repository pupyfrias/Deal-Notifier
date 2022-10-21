using System.Text.RegularExpressions;

namespace WebScraping.Extensions
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
            return Regex.Replace(str, "[,.+'\":;]", "", RegexOptions.Compiled);
        }
    }
}
