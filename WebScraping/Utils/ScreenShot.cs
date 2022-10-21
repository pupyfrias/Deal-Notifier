using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System.IO;
using WebScraping.Heplers;

namespace WebScraping.Utils
{
    public class ScreenShot
    {
        private static string basePath = Helper.basePath + "/Logs";

        /// <summary>
        /// Take a screenshot to the window.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="shop"></param>
        /// <param name="link"></param>
        /// <param name="counter"></param>
        public static void Take(IWebDriver driver, ref string shop, ref int link, ref int counter)
        {
            if (!Directory.Exists($"{basePath}/{shop}"))
            {
                Directory.CreateDirectory($"{basePath}/{shop}");
            }

            try
            {
                ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile($"{basePath}/{shop}/{link}_{counter - 1}.png", ScreenshotImageFormat.Png);
            }
            catch (WebDriverException e)
            {

                LogFile.Write<ScreenShot>(e.ToString());
                LogConsole.CreateLogger<Screenshot>().LogError(e.ToString());
            }
        }

        /// <summary>
        /// Take a screenshot at the bottom of the window.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="shop"></param>
        /// <param name="link"></param>
        /// <param name="counter"></param>
        /// <param name="by"></param>
        public static void TakeAtBottom(IWebDriver driver, ref string shop, ref int link, ref int counter, ref string by)
        {

            if (!Directory.Exists($"{basePath}/{shop}"))
            {
                Directory.CreateDirectory($"{basePath}/{shop}");
            }

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript($"document.{by}?.scrollIntoView();");
                ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile($"{basePath}/{shop}/{link}_{counter - 1}.png", ScreenshotImageFormat.Png);
            }
            catch (WebDriverException e)
            {
                LogFile.Write<ScreenShot>(e.ToString());
                LogConsole.CreateLogger<Screenshot>().LogError(e.ToString());
            }

        }
    }
}
