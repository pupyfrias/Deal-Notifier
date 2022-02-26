using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string url = "https://www.ebay.com/b/Cell-Phones-Smartphones/9355?Brand=HTC%7CHuawei%7CAlcatel%7CAmazon%7CApple%7CBLU%7CGoogle%7CLenovo%7CLG%7CMotorola%7CNokia%7CSamsung%7CRedmi%7CT%252DMobile%7CXiaomi%7CZTE&Connectivity=4G%7C4G%252B%7C5G%7CLTE&LH_BIN=1&LH_FS=1&LH_ItemCondition=1000&Operating%2520System=Android%7CiOS&RAM=2%2520GB%7C3%2520GB%7C16%2520GB%7C12%2520GB%7C8%2520GB%7C6%2520GB%7C4%2520GB&Screen%2520Size=5%252E5%2520%252D%25205%252E9%2520in%7C6%2520in%2520or%2520More&Storage%2520Capacity=128%2520GB%7C512%2520GB%7C64%2520GB%7C32%2520GB%7C256%2520GB&mag=1&rt=nc&_fsrp=0&_pgn=3&_sacat=9355&_udhi=120";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string first = url.Substring(0,url.IndexOf("&_pgn=") + 6);
            string page = url.Substring(url.IndexOf("&_pgn=") + 6);
            string last = page.Substring(page.IndexOf("&"));


            ChromeOptions options = new ChromeOptions();
            string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36";
            options.AddArguments("disable-infobars", "--no-sandbox", "--disable-dev-shm-usage", " --lang=en-us", $"--user-agent={userAgent}",
                  "--disable-gpu", "--disable-extensions", "--allow-running-insecure-content", "--ignore-certificate-errors",
                  "--window-size=1920,1080", "--disable-browser-side-navigation"/*, "--headless"*/);
            options.AddExcludedArgument("enable-automation");

            IWebDriver driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(url);
            Thread.Sleep(2000);
            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile($"{path}/Screenshot.png", ScreenshotImageFormat.Png);


            for (int i = 1; i < 10; i++)
            {
                string link = first + i + last;
                driver.Url = link;
                driver.Navigate();

                Thread.Sleep(2 * 1000);
            }

            Console.Read();
            driver.Quit();
        }
    }
}
