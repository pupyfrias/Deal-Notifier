using DataBase;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace WebScraping.Classes
{
    public class Method
    {
        public Method()
        {
            _logger = CreateLogger<Method>();

            
        }

        static string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/logs";
        static string logPath = $"{path}/log.txt";
        public static ArrayList checkList = new ArrayList();
        public static List<string> blackList = new List<string>();
        public static string[] conditionList = { "renovado", "renewed", "reacondicionado", "refurbished", "restaurado", "restored" };
        public static string[] filterList = { "tracfone", "total wireless", "net10", "simple mobile", "straight talk", "family mobile" };
        public static string[] includeList = { };//{ "huawei", "lg ", "moto", "xiaomi", "iphone", "samsung" };
        private static ILogger _logger;
        
        
        public static void Save(ref List<Models.Item> items)
        {
            using (WebScrapingEntities context = new WebScrapingEntities())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                items.ForEach(item =>
                {
                    var data = context.Items.SingleOrDefault(i => i.LINK == item.Link);
                    checkList.Add(item.Link);

                    if (data != null)
                    {
                        decimal oldPrice = (decimal)data.PRICE;
                        decimal saving = 0, saving_percent = 0;
                        bool validate = false;


                        if (oldPrice > item.Price)
                        {
                            saving = oldPrice - item.Price;
                            saving_percent = saving / oldPrice * 100;
                            validate = true;
                        }
                        else if (oldPrice < item.Price)
                        {
                            oldPrice = 0;
                            validate = true;
                        }
                        else if (item.Name != data.NAME)
                        {
                            validate = true;
                        }


                        if (validate)
                        {
                            context.SP_UPDATE_PRICE(data.ID, item.Price, oldPrice, saving, saving_percent, item.Name, item.Image);
                        }


                    }
                    else if (item.Save)
                    {
                        context.SP_ADD(item.Name, item.Price, item.Link, item.Condition, item.Shop, item.Image, item.Type);
                        
                    }
                });

                context.SaveChanges();

            }
        }
        public static string RemoveSpecialCharacters( string str)
        {
            return Regex.Replace(str, "[,.+'\":;]", "", RegexOptions.Compiled);
        }

        public static void ScreensShot(IWebDriver driver,ref string shop, ref int link,ref int counter,
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
                _logger.LogError(e.ToString());

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
                _logger.LogError(e.ToString());
            }
        }


    /*    public static void WriteLogs(string log)
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
           
        }*/

        public static void CreateLogDir()
        {
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            if (!File.Exists(logPath)) { using (FileStream fs = new FileStream(logPath, FileMode.Create)) ; }
        }


       /* public static void Print(string str, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(str);
        }*/


        public static ILogger CreateLogger<T>() where T : class
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddConsole();   
            });

            return loggerFactory.CreateLogger<T>();
        }

        public static ChromeDriverService CreateChromeDriverService()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.SuppressInitialDiagnosticInformation = true;
            return service;
        }
        public static ChromeOptions CreateChromeOptions( string option)
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

        public static ChromeDriver CreateChromeDriver(string option)
        {
            var options = CreateChromeOptions(option);
            var service = CreateChromeDriverService();
            return new ChromeDriver(service, options);
        }
    }
}
