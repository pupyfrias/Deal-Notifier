using DataBase;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
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
       

        public  static void  Selenium(string web)
        {
            ChromeOptions options = new ChromeOptions();
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36";
            options.AddArguments("disable-infobars", "--no-sandbox", "--disable-dev-shm-usage", " --lang=en-us", $"--user-agent={userAgent}",
                  "--disable-gpu", "--disable-extensions", "--allow-running-insecure-content", "--ignore-certificate-errors",
                  "--window-size=1920,1080", "--disable-browser-side-navigation", "--headless", "--log-level=3", "--silent",
                  "--enable-features=NetworkService,NetworkServiceInProcess");
            options.AddExcludedArgument("enable-automation");
            service.SuppressInitialDiagnosticInformation = true;

            if (web == "amazon")
            {
                var amazon = new Amazon();
                amazon.Run(options, service);
            }
            else if (web == "thestore")
            {
                var theStore = new TheStore();
                theStore.Run(options, service);
            }
            else if (web == "eBay")
            {
                var ebay = new Ebay();
                ebay.Run(options, service);
            }


        }

        public static void SaveOrUpdate(ref bool save, ref string name, string link, ref string image,
            ref decimal price,ref int condition,ref int shop, ref int type)
        {
            using (WebScrapingEntities context = new WebScrapingEntities())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                var data = context.Items.SingleOrDefault(i => i.LINK == link);
                checkList.Add(link);

                if (data != null)
                {
                    decimal oldPrice = (decimal)data.PRICE;
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
                        context.SP_UPDATE_PRICE(data.ID, price, oldPrice, saving, saving_percent, name, image);
                        context.SaveChanges();
                    }


                }
                else if (save)
                {
                    context.SP_ADD(name, price, link, condition, shop, image, type);
                    context.SaveChanges();
                }

            }
        }
        public static void RemoveSpecialCharacters(ref string str)
        {
            Regex.Replace(str, "[,.+'\":;]", "", RegexOptions.Compiled);
        }

        public static void ScreensShot(IWebDriver driver,ref string shop,ref int link,ref int counter,
            ref string by)
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
                WriteLogs($"ScreensShot ----> {e}");
            }



        }

        public static void ScreensShot(IWebDriver driver, ref string shop, ref int link,ref  int counter)
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
                WriteLogs($"ScreensShot ----> {e}");
            }
        }


        public static void WriteLogs(string log)
        {
            Task.Run( async() =>
            {
                EventWaitHandle waitHandle = new EventWaitHandle(true, EventResetMode.AutoReset, "SHARED_BY_ALL_PROCESSES");
                waitHandle.WaitOne();
                var sw = new StreamWriter(logPath, true);
                await sw.WriteLineAsync(log);
                sw.Close();
                waitHandle.Set();
            });
           
        }

        public static void CreateLogDir()
        {
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            if (!File.Exists(logPath)) { using (FileStream fs = new FileStream(logPath, FileMode.Create)) ; }
        }
    }
}
