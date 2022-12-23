using OpenQA.Selenium.Chrome;

namespace WebScraping.Core.Application.Utils
{
    public class SeleniumTools
    {
        /// <summary>
        /// Create chrome Driver Service
        /// </summary>
        /// <returns>ChromeDriverService</returns>
        private static ChromeDriverService CreateChromeDriverService()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.SuppressInitialDiagnosticInformation = true;
            return service;
        }

        /// <summary>
        /// Create a Chrome Options
        /// </summary>
        /// <param name="option">Chrome optons to add to Chrome Driever</param>
        /// <returns>ChromeOptions</returns>
        private static ChromeOptions CreateChromeOptions(string option)
        {
            string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36";
            ChromeOptions chromeOptions = new ChromeOptions();

            chromeOptions.AddArguments("disable-infobars", "--no-sandbox", "--disable-dev-shm-usage", " --lang=en-us", $"--user-agent={userAgent}",
                  "--disable-gpu", "--disable-extensions", "--allow-running-insecure-content", "--ignore-certificate-errors",
                  "--window-size=1920,1080", "--disable-browser-side-navigation", "--headless", "--log-level=3", "--silent",
                  "--enable-features=NetworkService,NetworkServiceInProcess", option);
            chromeOptions.AddExcludedArgument("enable-automation");
            return chromeOptions;
        }

        /// <summary>
        /// Create a Chrome Driver
        /// </summary>
        /// <param name="option"></param>
        /// <returns>ChromeDriver</returns>
        public static ChromeDriver CreateChromeDriver(string option)
        {
            var options = CreateChromeOptions(option);
            var service = CreateChromeDriverService();
            return new ChromeDriver(service, options);
        }
    }
}