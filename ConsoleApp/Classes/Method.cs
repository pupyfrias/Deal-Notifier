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

namespace ConsoleApp.Classes
{
    internal class Method
    {
        static string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/logs";
        public static ArrayList checkList = new ArrayList();
        public static List<string> blackList = new List<string>();
        public static string[] conditionList = { "renovado", "renewed", "reacondicionado", "refurbished", "restaurado", "restored" };
        public static string[] filterList = { "tracfone", "total wireless", "net10", "simple mobile", "straight talk" };
        public static string[] includeList = { };//{ "huawei", "lg ", "moto", "xiaomi", "iphone", "samsung" };

        public async static Task Selenium(string web, object[,] links)
        {
            await Task.Run(async () => {
                ChromeOptions options = new ChromeOptions();
                string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36";
                options.AddArguments("disable-infobars", "--no-sandbox", "--disable-dev-shm-usage", " --lang=en-us", $"--user-agent={userAgent}",
                      "--disable-gpu", "--disable-extensions", "--allow-running-insecure-content", "--ignore-certificate-errors",
                      "--window-size=1920,1080", "--disable-browser-side-navigation", "--headless", "--log-level=3", "--silent");
                options.AddExcludedArgument("enable-automation");

                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.SuppressInitialDiagnosticInformation = true;

                if (web == "amazon")
                {
                    var amazon = new Amazon();
                    await amazon.Run(links, options, service);
                }
                else if (web == "thestore")
                {
                    var theStore = new TheStore();
                    await theStore.Run(links, options, service);
                }
                else if (web == "eBay")
                {
                    var ebay = new Ebay();
                    await ebay.Run(links, options, service);
                }
            });

        }

        public static async Task SaveOrUpdate(bool save, string name, string link, string image, decimal price,
            int condition, string shop, int type)
        {
            using (WebScrapingEntities context = new WebScrapingEntities())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                //var data = await Task.Run(() => context.SP_GET_ONE(link).AsEnumerable().FirstOrDefault());
                var data = await context.Items.SingleOrDefaultAsync(i => i.LINK == link);
                checkList.Add(link);

                if (data != null)
                {
                    decimal oldPrice = (decimal)((decimal)data.OLD_PRICE > 0 ? data.OLD_PRICE : data.PRICE);
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


                    if (true)//validate)
                    {
                        context.SP_UPDATE_PRICE(data.ID, price, oldPrice, saving, saving_percent, name, type);//quitar type
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

        public static void ScreensShot(IWebDriver driver, string shop, int counter)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile($"{path}/{shop}_{counter}.png", ScreenshotImageFormat.Png);
        }

        public async static Task WriteLogs(string log, string shop)
        {
            EventWaitHandle waitHandle = new EventWaitHandle(true, EventResetMode.AutoReset, "SHARED_BY_ALL_PROCESSES");
            string logPath = $"{path}/log {shop}.txt"; /*{DateTime.Now.ToShortDateString().Replace("/", "_")}.txt"*/

            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            if (!File.Exists(logPath)) { using (FileStream fs = new FileStream(logPath, FileMode.Create)) ; }

            waitHandle.WaitOne();
            var sw = new StreamWriter(logPath, true);
            await sw.WriteLineAsync(log);
            sw.Close();
            waitHandle.Set();
        }
    }
}
