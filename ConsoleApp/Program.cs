using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using DataBase;
using System.Text.RegularExpressions;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Threading;
using System.Media;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp
{
    class Program
    {
        static ArrayList checkList = new ArrayList();
        static string[] conditionList = { "renovado", "renewed", "reacondicionado", "refurbished", "restaurado", "restored" };
        static string[] filterList = { "tracfone", "total wireless", "net10", "simple mobile", "straight talk", "verizon", "soporte", "carcasa", "funda", "cover", "carcasa" };
        static string[] includeList = { "huawei", "lg ", "moto", "xiaomi", "iphone", "samsung" };
        



        static async Task Main(string[] args)
        {
            var listSmartPhone = new List<SmartPhone>();
            try
            {
               var task1 = Task.Run(async () =>
                {
                    await Selenium("https://www.amazon.com/-/es/s?i=mobile&bbn=7072561011&rh=n%3A7072561011%2Cp_36%3A-30000%2Cp_72%3A2491149011&dc&fs=true&language=es&qid=1614961572&rnid=2491147011&ref=sr_pg_");
                });
                var task2 = Task.Run(async () =>
                {
                    await Selenium("https://thestore.com/c/refurbished-cell-phones-58");
                });

                await Task.WhenAll(task1, task2);
                
                using (WebScrapingEntities context = new WebScrapingEntities())
                {
                    context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                    listSmartPhone = await context.SmartPhone.ToListAsync();
                }

                Parallel.ForEach(listSmartPhone, (i) =>
                {
                    if (checkList.Contains(i.LINK))
                    {
                        using (WebScrapingEntities context = new WebScrapingEntities())
                        {
                            context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                            context.SP_UPDATE_STATUS(i.ID, true);
                        }  
                    }
                    else
                    {
                        using (WebScrapingEntities context = new WebScrapingEntities())
                        {
                            context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                            context.SP_UPDATE_STATUS(i.ID, false);
                        }
                    }

                });

                SystemSounds.Asterisk.Play();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
            
           
            
        }
        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[,.+'\":;]", "", RegexOptions.Compiled);
        }
        private async static Task Selenium(String url)
        {
            await Task.Run(() => {

                ChromeOptions options = new ChromeOptions();
                options.AddArguments("start-maximized", "disable-infobars", "--no-sandbox", "--disable-dev-shm-usage",
                      "--disable-gpu", "--disable-extensions", "--allow-running-insecure-content", "--ignore-certificate-errors","headless");

                if (url.Contains("amazon.com"))
                {
                    Amazon(url, options);
                }
                else if (url.Contains("thestore.com"))
                {
                    TheStore(url, options);
                }
            });
            
        }
         
        public static async Task SaveOrUpdate(bool save, string name, string link, string image, decimal price, int condition,string shop)
        {
            using (WebScrapingEntities context = new WebScrapingEntities())
            {
                var data = await context.SmartPhone.FirstOrDefaultAsync(i => i.LINK == link);

                if (data != null)
                {
                    checkList.Add(link);
                    decimal oldPrice = (decimal)data.PRICE;
                    if (oldPrice != price)
                    {
                        decimal saving = oldPrice - price > 0 ? oldPrice - price : 0;
                        context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                        context.SP_UPDATE_PRICE(data.ID, price, oldPrice, saving);
                        await context.SaveChangesAsync();
                    }
                }
                else if (save)
                {
                    context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                    context.SP_ADD(name, price, link, condition, shop, image);
                    await context.SaveChangesAsync();
                }
                
            }
        }

        public static void Amazon(string url, ChromeOptions options)
        {
            
            options.AddArgument($"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}/User Data/Amazon");
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(url);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                while (true)
                {
                    By selector = By.CssSelector("div[data-component-type='s-search-result']");

                    wait.Until(ExpectedConditions.ElementIsVisible(selector));
                    ReadOnlyCollection<IWebElement> elements = driver.FindElements(selector);
                    Parallel.ForEach(elements, async (element) =>
                    {
                        try
                        {
                            IWebElement eName = element.FindElement(By.XPath(".//h2/a/span"));
                            IWebElement eLink = element.FindElement(By.XPath(".//h2/a"));
                            IWebElement eImage = element.FindElement(By.ClassName("s-image"));
                            IWebElement ePriceWhole = element.FindElement(By.CssSelector("span[class='a-price-whole']"));
                            IWebElement ePriceFraction = element.FindElement(By.ClassName("a-price-fraction"));
                            string name = RemoveSpecialCharacters(eName.Text);
                            string link = eLink.GetAttribute("href");
                            link = link.Substring(0, link.IndexOf("/ref"));
                            string image = eImage.GetAttribute("src");
                            decimal price = decimal.Parse(ePriceWhole.Text.Replace(",","") + "," + ePriceFraction.Text);
                            int condition = 1;
                            bool save = true;
                            bool valide = false;
                            string shop = "Amazon";

                            Parallel.ForEach(conditionList, (data, state) =>
                            {
                                if (name.ToLower().Contains(data))
                                {
                                    condition = 0;
                                    state.Break();
                                }
                            });

                            Parallel.ForEach(filterList, (data, state) =>
                            {
                                if (name.ToLower().Contains(data))
                                {
                                    save = false;
                                    state.Break();
                                }
                            });
                            
                            Parallel.ForEach(includeList, (data, state) =>
                            {
                                if (name.ToLower().Contains(data))
                                {
                                    valide = true;
                                    state.Break();
                                }
                                
                            });

                            if(valide && price>25 && save)
                            {
                                save = true;
                            }
                            else
                            {
                                save = false;
                            }

                            await SaveOrUpdate(save, name, link, image, price, condition, shop);

                        }
                        catch (NoSuchElementException)
                        {
                        }

                    });

                    Console.WriteLine("\nAmazon");

                    try
                    {
                        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("li[class='a-last'"))).Click();
                    }
                    catch (WebDriverTimeoutException e)
                    {
                        break;
                    }

                }

            }
        }

        public static void TheStore(string url, ChromeOptions options)
        {
            options.AddArgument($"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}/User Data/The Store");
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(url);
                bool removeElement = true;

               
                while (true)
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                    By selector = By.CssSelector("div[class='column is-6-mobile is-6-tablet is-4-desktop product-listing__item']");
                    wait.Until(ExpectedConditions.ElementToBeClickable(selector));
                    Thread.Sleep(2000);
                 
                    ReadOnlyCollection<IWebElement> elements = driver.FindElements(selector);
                    Parallel.ForEach(elements, async (element) =>
                    {
                        try
                        {
                            IWebElement eName = element.FindElement(By.ClassName("product-tile__name"));
                            IWebElement eLink = element.FindElement(By.ClassName("product-tile__link"));
                            IWebElement eImage = element.FindElement(By.XPath(".//div/a/div/img"));
                            IWebElement ePriceWhole = element.FindElement(By.CssSelector("span[class='amount']"));
                            IWebElement conditionName = element.FindElement(By.ClassName("product-tile__condition"));
                            string name = $"{RemoveSpecialCharacters(eName.Text)} ({conditionName.Text})";
                            string link = eLink.GetAttribute("href");
                            string image = "https://thestore.com"+eImage.GetAttribute("data-src");
                            decimal price = decimal.Parse(ePriceWhole.Text.Replace("$","").Replace(".",","));
                            int condition = 1;
                            bool save = true;
                            bool valide = true;
                            string shop = "TheStore";

                            Parallel.ForEach(conditionList, (data, state) =>
                            {
                                if (name.ToLower().Contains(data))
                                {
                                    condition = 0;
                                    state.Break();
                                }
                            });

                            Parallel.ForEach(filterList, (data, state) =>
                            {
                                if (name.ToLower().Contains(data))
                                {
                                    save = false;
                                    state.Break();
                                }
                            });


                            Parallel.ForEach(includeList, (data, state) =>
                            {
                                if (name.ToLower().Contains(data))
                                {
                                    valide = true;
                                    state.Break();
                                }

                            });

                            if (valide && price > 25 && save)
                            {
                                save = true;
                            }
                            else
                            {
                                save = false;
                            }

                            await SaveOrUpdate(save, name, link, image, price, condition, shop);

                        }
                        catch (NoSuchElementException)
                        {
                        }

                    });

                    Console.WriteLine("\nThe Store");

                    try
                    {
                        if (removeElement)
                        {
                            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                            try { js.ExecuteScript("document.getElementsByClassName('acsb-trigger acsb-bg-lead acsb-trigger-size-medium acsb-trigger-position-x-right acsb-trigger-position-y-bottom acsb-ready').forEach(e=> e.remove())"); } catch (Exception) { }
                            try { js.ExecuteScript("document.getElementById('cc-button').remove()"); } catch (Exception) { }
                            try { js.ExecuteScript("document.getElementsByClassName('hello-bar').forEach(e=> e.remove())"); } catch (Exception) { }
                            removeElement = false;
                        }
                        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[class='round-button round-button__next']"))).Click();
                    }
          
                    catch (WebDriverTimeoutException e)
                    {
                        break;
                    }

                }

            }

        }
    }
}
