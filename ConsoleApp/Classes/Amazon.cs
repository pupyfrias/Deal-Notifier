using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;



namespace ConsoleApp
{
    internal class Amazon: Program
    {
        public Amazon(string url, ChromeOptions options)
        {
            options.AddArgument($"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}/User Data/Amazon");
            using (IWebDriver driver = new ChromeDriver(options))
            {
                int counter = 1;
                driver.Navigate().GoToUrl(url);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
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
                            decimal price = decimal.Parse(ePriceWhole.Text.Replace(",", "") + "." + ePriceFraction.Text);
                            int condition = 1;
                            bool save = true;
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

                        
                            await SaveOrUpdate(save, name, link, image, price, condition, shop);

                        }
                        catch (Exception e) when (e is NoSuchElementException | e is StaleElementReferenceException)
                        {
                            Console.Write($"ERROR AMAZON: ---> {e.Message}\n");
                        }

                    });

                    Console.WriteLine($"\nAmazon {counter} | {amazonLinkPositon}");
                    counter++;

                    try
                    {
                        wait2.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[class='s-pagination-item s-pagination-next s-pagination-button s-pagination-separator'"))).Click();

                    }
                    catch (WebDriverTimeoutException e)
                    {
                        Console.Write("NOT FOUND");
                        break;
                    }

                }

            }

        }   
    }
}
