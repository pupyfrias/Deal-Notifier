using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using DataBase;
using System.Text.RegularExpressions;

namespace ConsoleApp
{
    class Program
    {
        

        static void Main(string[] args)
        {
           Selenium();
            
        }
        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[,.+'\":;]", "", RegexOptions.Compiled);
        }
        private static void Selenium()
        {
            string[] conditionList = { "renovado", "renewed", "reacondicionado", "refurbished", "restaurado" };
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("start-maximized","disable-infobars", "--no-sandbox", "--disable-dev-shm-usage",
                "--disable-gpu", "--disable-extensions", "--allow-running-insecure-content", "--ignore-certificate-errors",
                $"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}/User Data");

            using (IWebDriver driver = new ChromeDriver(options))
            {
                //string url = "https://www.amazon.com/-/es/s?i=mobile&bbn=7072561011&rh=n%3A7072561011%2Cp_36%3A-30000%2Cp_72%3A2491149011&dc&fs=true&language=es&qid=1614961572&rnid=2491147011&ref=sr_pg_";
                string url = "https://www.amazon.com/-/es/s?i=mobile&bbn=7072561011&rh=n%3A7072561011%2Cp_36%3A-30000%2Cp_72%3A2491149011&dc&fs=true&page=95&language=es&qid=1639093987&rnid=2491147011&ref=sr_pg_94";
                driver.Navigate().GoToUrl(url);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                while (true)
                {
                    

                    By selector = By.CssSelector("div[data-component-type='s-search-result']");

                    wait.Until(ExpectedConditions.ElementIsVisible(selector));
                    ReadOnlyCollection<IWebElement> elements = driver.FindElements(selector);
                    int i = 1;
                    foreach (IWebElement element in elements)
                    {
                        try
                        {
                            Console.WriteLine(i);
                            i++;
                            IWebElement eName = element.FindElement(By.XPath(".//h2/a/span"));
                            IWebElement eLink = element.FindElement(By.XPath(".//h2/a"));
                            IWebElement eImage = element.FindElement(By.ClassName("s-image"));
                            IWebElement ePriceWhole = element.FindElement(By.CssSelector("span[class='a-price-whole']"));
                            IWebElement ePriceFraction = element.FindElement(By.ClassName("a-price-fraction"));
                            string name = RemoveSpecialCharacters(eName.Text);                     
                            string link = eLink.GetAttribute("href");
                            link = link.Substring(0, link.IndexOf("/ref"));
                            string image = eImage.GetAttribute("src");
                            decimal price = decimal.Parse(ePriceWhole.Text + "," + ePriceFraction.Text);
                            int condition = 1;
                           
                            foreach(string data in conditionList)
                            {
                                if (name.Contains(data))
                                {
                                    condition = 0;
                                    break;
                                }
                            }


                            using (WebScrapingEntities context = new WebScrapingEntities())
                            {
                                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                                context.SP_ADD(name, price, link, condition, "Amazon", image);
                                context.SaveChanges();
                            }
                            Console.WriteLine("Done");
                        }
                        catch (NoSuchElementException e)
                        {
                            Console.WriteLine(e.Message);
                        }

                    }
                    
                    
                    try
                    {
                        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("li[class='a-last'"))).Click();
                    }
                    catch (WebDriverTimeoutException e)
                    {
                        Console.ReadLine();
                        break;
                    }
                    
                }
                
            }
    
        }
    }
}
