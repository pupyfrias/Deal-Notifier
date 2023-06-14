using DealNotifier.Core.Application.Heplers;
using DealNotifier.Core.Application.Utils;
using OpenQA.Selenium;

namespace DealNotifier.Core.Application.Extensions
{
    public static class ScreenShotExtension
    {
        private static string screenShotbasePath = Helper.BasePath + "/Logs/ScreenShots";

        /// <summary>
        /// TakeScreenShot a screenshot to the window.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="shop"></param>
        /// <param name="link"></param>
        /// <param name="counter"></param>
        public static void TakeScreenShot(this IWebDriver driver, ref string shop, ref int link, ref int counter)
        {
            if (!Directory.Exists($"{screenShotbasePath}/{shop}"))
            {
                Directory.CreateDirectory($"{screenShotbasePath}/{shop}");
            }

            try
            {
                ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile($"{screenShotbasePath}/{shop}/{link}_{counter - 1}.png", ScreenshotImageFormat.Png);
            }
            catch (WebDriverException e)
            {
                Logger.CreateLogger()
                    .Error(e.ToString());
            }
        }

        /// <summary>
        /// TakeScreenShot a screenshot at the bottom of the window.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="shop"></param>
        /// <param name="link"></param>
        /// <param name="counter"></param>
        /// <param name="by"></param>
        public static void TakeScreemShotAtBottom(this IWebDriver driver, ref string shop, ref int link, ref int counter, ref string by)
        {
            if (!Directory.Exists($"{screenShotbasePath}/{shop}"))
            {
                Directory.CreateDirectory($"{screenShotbasePath}/{shop}");
            }

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript($"document.{by}?.scrollIntoView();");
                ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile($"{screenShotbasePath}/{shop}/{link}_{counter - 1}.png", ScreenshotImageFormat.Png);
            }
            catch (WebDriverException e)
            {
                Logger.CreateLogger()
                    .Error(e.ToString());
            }
        }
    }
}