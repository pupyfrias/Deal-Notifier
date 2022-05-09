using DataBase;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WebScraping.Classes
{
    internal class Method
    {
        static string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/logs";
        static string logPath = $"{path}/log.txt";
        public static ArrayList checkList = new ArrayList();
        public static List<string> blackList = new List<string>();
        public static string[] conditionList = { "renovado", "renewed", "reacondicionado", "refurbished", "restaurado", "restored" };
        public static string[] filterList = { "tracfone", "total wireless", "net10", "simple mobile", "straight talk", "family mobile" };
        public static string[] includeList = { };//{ "huawei", "lg ", "moto", "xiaomi", "iphone", "samsung" };

        public async static Task Selenium(string web)
        {
            await Task.Run(async () =>
            {
                ChromeOptions options = new ChromeOptions();
                string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36";
                options.AddArguments("disable-infobars", "--no-sandbox", "--disable-dev-shm-usage", " --lang=en-us", $"--user-agent={userAgent}",
                      "--disable-gpu", "--disable-extensions", "--allow-running-insecure-content", "--ignore-certificate-errors",
                      "--window-size=1920,1080", "--disable-browser-side-navigation", "--headless", "--log-level=3", "--silent"
                      ,"--enable-features=NetworkService,NetworkServiceInProcess");

                options.AddExcludedArgument("enable-automation");

                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.SuppressInitialDiagnosticInformation = true;

                if (web == "amazon")
                {
                    var amazon = new Amazon();
                    await amazon.Run(options, service);
                }
                else if (web == "thestore")
                {
                    var theStore = new TheStore();
                    await theStore.Run(options, service);
                }
                else if (web == "eBay")
                {
                    var ebay = new Ebay();
                    await ebay.Run(options, service);
                }
            });

        }

        public static async Task SaveOrUpdate(bool save, string name, string link, string image, decimal price,
            int condition, int shop, int type)
        {
            using (WebScrapingEntities context = new WebScrapingEntities())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                var data = await context.Items.SingleOrDefaultAsync(i => i.LINK == link);
                checkList.Add(link);

                if (data != null)
                {
                    decimal oldPrice = (decimal)((decimal)data.PRICE);
                    decimal saving = 0, saving_percent = 0;
                    bool validate = false;


                    if (oldPrice > price)
                    {
                        saving = oldPrice - price;
                        saving_percent = saving / oldPrice * 100;
                        validate = true;
                    }
                    else if (oldPrice < price)
                    {
                        oldPrice = 0;
                        validate = true;
                    }
                    else if (name != data.NAME)
                    {
                        validate = true;
                    }


                    if (validate)
                    {
                        context.SP_UPDATE_PRICE(data.ID, price, oldPrice, saving, saving_percent, name);
                        await context.SaveChangesAsync();
                    }


                }
                else if (save)
                {
                    context.SP_ADD(name, price, link, condition, shop, image, type);
                    await context.SaveChangesAsync();
                }

            }
        }
        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[,.+'\":;]", "", RegexOptions.Compiled);
        }

        public static async Task ScreensShot(IWebDriver driver, string shop, int link, int counter, string by)
        {
            await Task.Run(async () =>
           {
               if (!Directory.Exists($"{path}/{shop}"))
               {
                   Directory.CreateDirectory($"{path}/{shop}");
               }

               try
               {
                   IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                   js.ExecuteScript($"document.{by}?.scrollIntoView();"); 
                   ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile($"{path}/{shop}/{link}_{counter - 1}.png", ScreenshotImageFormat.Png);
               }
               catch (WebDriverException e)
               {
                   await WriteLogs($"ScreensShot ----> {e}");
               }

           });

        }

        public static async Task ScreensShot(IWebDriver driver, string shop, int link, int counter)
        {
            await Task.Run(async () =>
            {
                if (!Directory.Exists($"{path}/{shop}"))
                {
                    Directory.CreateDirectory($"{path}/{shop}");
                }

                try
                {
                    ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile($"{path}/{shop}/{link}_{counter - 1}.png", ScreenshotImageFormat.Png);
                }
                catch (WebDriverException e)
                {
                    await WriteLogs($"ScreensShot ----> {e}");
                }

            });

        }


        public async static Task WriteLogs(string log)
        {
            EventWaitHandle waitHandle = new EventWaitHandle(true, EventResetMode.AutoReset, "SHARED_BY_ALL_PROCESSES");
            waitHandle.WaitOne();
            var sw = new StreamWriter(logPath, true);
            await sw.WriteLineAsync(log);
            sw.Close();
            waitHandle.Set();
        }

        public static void CreateLogDir()
        {
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            if (!File.Exists(logPath)) { using (FileStream fs = new FileStream(logPath, FileMode.Create)) ; }
        }
    }
}
